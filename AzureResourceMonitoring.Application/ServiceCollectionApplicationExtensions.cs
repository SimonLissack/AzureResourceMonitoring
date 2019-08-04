using AzureResourceMonitoring.Infrastructure.Azure;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AzureResourceMonitoring.Application
{
    public static class ServiceCollectionApplicationExtensions
    {
        public static IServiceCollection AddAzureInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var keyVaultConfig = new KeyVaultConfiguration();
            configuration.Bind(ConfigurationPrefixes.KeyVault, keyVaultConfig);

            return services
                .AddSingleton(keyVaultConfig)
                .AddAzureServices();
        }
    }
}