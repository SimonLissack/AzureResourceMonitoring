namespace AzureResourceMonitoring.Infrastructure.Azure.Authentication
{
    public class AzureCredentials
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
        public string SubscriptionId { get; set; }
        public string TenantId { get; set; }
    }
}