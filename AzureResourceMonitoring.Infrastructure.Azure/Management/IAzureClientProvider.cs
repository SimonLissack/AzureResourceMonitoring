using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;

namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public interface IAzureClientProvider
    {
        Task<IAzure> CreateClient();
    }
}