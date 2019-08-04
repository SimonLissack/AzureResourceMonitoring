using System;
using System.IO;
using System.Threading.Tasks;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using Microsoft.Extensions.Configuration;

namespace AzureResourceMonitoring.Application
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets<Program>(optional: true)
                .Build();

            var keyVaultConfig = new KeyVaultConfiguration();
            configuration.Bind(ConfigurationKeys.KeyVaultPrefix, keyVaultConfig);

            var servicePrincipalProvider = new ServicePrincipalProvider(keyVaultConfig);

            var credentials = await servicePrincipalProvider.GetCredentialsFromKeyVault();

            Console.WriteLine($"Client: {credentials.ClientId}");
            Console.WriteLine($"Tenant: {credentials.TenantId}");
        }

        static class ConfigurationKeys
        {
            public const string KeyVaultPrefix = "KeyVault";
        }
    }
}
