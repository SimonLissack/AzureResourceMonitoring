using System.Threading.Tasks;

namespace AzureResourceMonitoring.Infrastructure.Azure.Authentication
{
    public interface IServicePrincipalProvider
    {
        Task<ServicePrincipalCredentials> GetCredentialsFromKeyVault();
    }
}