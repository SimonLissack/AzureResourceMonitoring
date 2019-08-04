using System.Threading.Tasks;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Newtonsoft.Json;

namespace AzureResourceMonitoring.Infrastructure.Azure.Authentication
{
    public class ServicePrincipalProvider
    {
        readonly KeyVaultConfiguration _keyVaultConfiguration;

        public ServicePrincipalProvider(KeyVaultConfiguration keyVaultConfiguration)
        {
            _keyVaultConfiguration = keyVaultConfiguration;
        }

        public async Task<ServicePrincipalCredentials> GetCredentialsFromKeyVault()
        {
            var azureTokenProvider = new AzureServiceTokenProvider();

            var client = new KeyVaultClient(new KeyVaultClient.AuthenticationCallback(azureTokenProvider.KeyVaultTokenCallback));

            var secret = await client.GetSecretAsync(_keyVaultConfiguration.SecretUri).ConfigureAwait(false);

            return JsonConvert.DeserializeObject<ServicePrincipalCredentials>(secret.Value);
        }
    }
}