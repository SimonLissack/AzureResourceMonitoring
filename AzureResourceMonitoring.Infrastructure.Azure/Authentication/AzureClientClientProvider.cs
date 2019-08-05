using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using AzureManager = Microsoft.Azure.Management.Fluent.Azure;

namespace AzureResourceMonitoring.Infrastructure.Azure.Authentication
{
    public class AzureClientClientProvider : IAzureClientProvider
    {
        readonly IServicePrincipalProvider _servicePrincipalProvider;

        public AzureClientClientProvider(IServicePrincipalProvider servicePrincipalProvider)
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