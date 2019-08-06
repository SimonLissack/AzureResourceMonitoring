using System.Threading.Tasks;
using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using AzureManager = Microsoft.Azure.Management.Fluent.Azure;

namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public class AzureClientProvider : IAzureClientProvider
    {
        readonly IServicePrincipalProvider _servicePrincipalProvider;

        public AzureClientProvider(IServicePrincipalProvider servicePrincipalProvider)
        {
            _servicePrincipalProvider = servicePrincipalProvider;
        }

        public async Task<IAzure> CreateClient()
        {
            var servicePrincipal = await _servicePrincipalProvider.GetCredentialsFromKeyVault();

            var credentials = new AzureCredentialsFactory()
                .FromServicePrincipal(
                    servicePrincipal.ClientId,
                    servicePrincipal.ClientSecret,
                    servicePrincipal.TenantId,
                    AzureEnvironment.AzureGlobalCloud
                );

            return await AzureManager
                .Authenticate(credentials)
                .WithDefaultSubscriptionAsync();
        }
    }
}