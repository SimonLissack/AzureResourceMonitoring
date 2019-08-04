using System;
using System.IO;
using System.Threading.Tasks;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

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

            var credentials = await GetCredentialsFromKeyVault(keyVaultConfig);

            Console.WriteLine($"Client: {credentials.ClientId}");
            Console.WriteLine($"Tenant: {credentials.TenantId}");
        }

        static async Task<ServicePrincipalCredentials> GetCredentialsFromKeyVault(KeyVaultConfiguration configuration)
        {
            var secretUri = configuration.SecretUri;
            var azureTokenProvider = new AzureServiceTokenProvider();

            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureTokenProvider.KeyVaultTokenCallback));

            Console.WriteLine($"Looking for secret at: {secretUri}");

            var secret = await client.GetSecretAsync(secretUri).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<ServicePrincipalCredentials>(secret.Value);
        }

        static class ConfigurationKeys
        {
            public const string KeyVaultPrefix = "KeyVault";
        }
    }
}
