using System.Threading.Tasks;
using Microsoft.Azure.Management.Fluent;

namespace AzureResourceMonitoring.Infrastructure.Azure.Authentication
{
    public interface IAzureClientProvider
    {
        Task<IAzure> CreateClient();
    }
}