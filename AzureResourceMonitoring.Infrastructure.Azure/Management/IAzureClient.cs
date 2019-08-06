using System.Collections.Async;

namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public interface IAzureClient
    {
        IAsyncEnumerable<AzureResource> GetResources();
    }
}