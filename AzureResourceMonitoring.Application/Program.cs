using System.Collections.Async;
using System.IO;
using System.Threading.Tasks;
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
                await azureClient.GetResources().Take(10).ForEachAsync(item =>
                {
                    logger.LogInformation($"{item.Name} in resource group {item.ResourceGroup}. Type: {item.Type}. Region: {item.Region}");
                });
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
