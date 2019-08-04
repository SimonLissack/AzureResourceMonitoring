using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace AzureResourceMonitoring
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appSettings.json", optional: true, reloadOnChange: true)
                .AddUserSecrets(typeof(Program).Assembly, optional: true)
                .Build();

            var credentials = await GetCredentialsFromKeyVault(configuration);

            Console.WriteLine($"Client: {credentials.ClientId}");
            Console.WriteLine($"Tenant: {credentials.TenantId}");
        }

        static async Task<AzureCredentials> GetCredentialsFromKeyVault(IConfigurationRoot configuration)
        {
            var secretUri = configuration[ConfigurationKeys.SecretName];
            var azureTokenProvider = new AzureServiceTokenProvider();

            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureTokenProvider.KeyVaultTokenCallback));

            Console.WriteLine($"Looking for secret at: {secretUri}");

            var secret = await client.GetSecretAsync(secretUri).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<AzureCredentials>(secret.Value);
        }

        static class ConfigurationKeys
        {
            public const string SecretName = "KeyVault:SecretUri";
        }

        public class AzureCredentials
        {
            public string ClientId { get; set; }
            public string ClientSecret { get; set; }
            public string SubscriptionId { get; set; }
            public string TenantId { get; set; }
        }
    }
}
