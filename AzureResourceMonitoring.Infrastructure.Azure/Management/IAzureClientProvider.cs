using System.Threading.Tasks;

namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public interface IAzureClientProvider
    {
        Task<IAzureClient> CreateClient();
    }
}