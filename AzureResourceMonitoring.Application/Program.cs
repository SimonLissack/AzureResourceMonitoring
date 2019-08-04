using System;
using System.IO;
using System.Threading.Tasks;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
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
            var serviceCollection = SetupServiceCollection();

            ConfigureAzureInfrastructure(configuration, serviceCollection);

            using (var serviceProvider = serviceCollection.BuildServiceProvider())
            {
                var logger = serviceProvider.GetService<ILogger<Program>>();

                var servicePrincipalProvider = serviceProvider.GetService<IServicePrincipalProvider>();
                var credentials = await servicePrincipalProvider.GetCredentialsFromKeyVault();

                logger.LogInformation($"Client: {credentials.ClientId}");
                logger.LogInformation($"Tenant: {credentials.TenantId}");
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

        static IServiceCollection SetupServiceCollection()
        {
            return new ServiceCollection()
                .AddLogging(c => c.AddConsole().SetMinimumLevel(LogLevel.Debug));
        }

        static void ConfigureAzureInfrastructure(IConfiguration configuration, IServiceCollection services)
        {
            var keyVaultConfig = new KeyVaultConfiguration();
            configuration.Bind(ConfigurationKeys.KeyVaultPrefix, keyVaultConfig);

            services
                .AddSingleton(keyVaultConfig)
                .AddTransient<IServicePrincipalProvider, ServicePrincipalProvider>();
        }

        static class ConfigurationKeys
        {
            public const string KeyVaultPrefix = "KeyVault";
        }
    }
}
