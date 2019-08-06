namespace AzureResourceMonitoring.Infrastructure.Azure.Management
{
    public class AzureResource
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string ResourceGroup { get; set; }
        public string Region { get; set; }
        public string Id { get; set; }
    }
}