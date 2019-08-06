using System.IO;
using System.Threading.Tasks;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using AzureResourceMonitoring.Infrastructure.Azure.Management;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AzureResourceMonitoring.Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = SetupConfiguration();
            var serviceCollection = SetupServiceCollection(configuration);

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var logger = serviceProvider.GetService<ILogger<Program>>();

                var azureClient = await serviceProvider.GetService<IAzureClientProvider>().CreateClient();

                logger.LogInformation("Resource groups:");
                foreach (var resourceGroup in await azureClient.ResourceGroups.ListAsync())
                {
                    logger.LogInformation(resourceGroup.Name);
                }
            }
        }

        static IConfiguration SetupConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>(optional: true)
                .Build();
        }

        static IServiceCollection SetupServiceCollection(IConfiguration configuration)
        {
            return new ServiceCollection()
                .AddAzureInfrastructure(configuration)
                .AddLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Debug));
        }
    }
}
