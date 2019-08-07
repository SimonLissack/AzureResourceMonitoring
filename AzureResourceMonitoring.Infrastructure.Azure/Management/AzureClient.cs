using System.Collections.Async;
using Microsoft.Azure.Management.ResourceManager.Fluent;

namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public class AzureClient : IAzureClient
    {
        readonly IResourceManager _client;

        public AzureClient(IResourceManager client)
        {
            _client = client;
        }

        public IAsyncEnumerable<AzureResource> GetResources() => new AsyncEnumerable<AzureResource>(async yield =>
        {
            foreach (var resource in await _client.GenericResources.ListAsync())
            {
                await yield.ReturnAsync(new AzureResource
                {
                    Name = resource.Name,
                    ResourceGroup = resource.ResourceGroupName,
                    Type = resource.Type,
                    Region = resource.RegionName,
                    Id = resource.Id
                });
            }
        });
    }
}