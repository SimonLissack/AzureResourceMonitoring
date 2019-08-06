using System.Threading.Tasks;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;

namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public class AzureClientProvider : IAzureClientProvider
    {
        readonly IServicePrincipalProvider _servicePrincipalProvider;

        public AzureClientProvider(IServicePrincipalProvider servicePrincipalProvider)
        {
            _servicePrincipalProvider = servicePrincipalProvider;
        }

        public async Task<IAzureClient> CreateClient()
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

            return new AzureClient(resourceManager);
        }
    }
}