using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace AzureResourceMonitoring.Infrastructure.Azure.Authentication
{
    public class ServicePrincipalProvider : IServicePrincipalProvider
    {
        readonly ILogger<ServicePrincipalProvider> _logger;
        readonly KeyVaultConfiguration _keyVaultConfiguration;

        public ServicePrincipalProvider(ILogger<ServicePrincipalProvider> logger, KeyVaultConfiguration keyVaultConfiguration)
        {
            _logger = logger;
            _keyVaultConfiguration = keyVaultConfiguration;
        }

        public async Task<ServicePrincipalCredentials> GetCredentialsFromKeyVault()
        {
            _logger.LogDebug($"Getting service principal details from Azure Key Vault using secret URI: {_keyVaultConfiguration.SecretUri}");

            var azureTokenProvider = new AzureServiceTokenProvider();

            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureTokenProvider.KeyVaultTokenCallback));

            var secret = await client.GetSecretAsync(_keyVaultConfiguration.SecretUri).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<ServicePrincipalCredentials>(secret.Value);
        }
    }
}