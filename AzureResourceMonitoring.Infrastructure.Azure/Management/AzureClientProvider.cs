using System.Threading.Tasks;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public class AzureClientProvider : IAzureClientProvider
    {
        readonly IServicePrincipalProvider _servicePrincipalProvider;

        IAzureClient _cachedClient;

        public AzureClientProvider(IServicePrincipalProvider servicePrincipalProvider)
        {
            _servicePrincipalProvider = servicePrincipalProvider;
        }

        public async Task<IAzureClient> CreateClient()
        {
            if (_cachedClient == null)
            {
                var servicePrincipal = await _servicePrincipalProvider.GetCredentialsFromKeyVault();

                var credentials = new AzureCredentialsFactory()
                    .FromServicePrincipal(
                        servicePrincipal.ClientId,
                        servicePrincipal.ClientSecret,
                        servicePrincipal.TenantId,
                        AzureEnvironment.AzureGlobalCloud
                    );

                var resourceManager = ResourceManager
                    .Authenticate(credentials)
                    .WithSubscription(servicePrincipal.SubscriptionId);

                _cachedClient = new AzureClient(resourceManager);
            }

            return _cachedClient;
        }
    }
}