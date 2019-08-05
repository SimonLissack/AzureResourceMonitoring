using AzureResourceMonitoring.Infrastructure.Azure.Authentication;
using Microsoft.Extensions.DependencyInjection;

namespace AzureResourceMonitoring.Infrastructure.Azure
{
    public static class ServiceCollectionAzureExtensions
    {
        public static IServiceCollection AddAzureServices(this IServiceCollection services)
        {
            return services
                .AddTransient<IServicePrincipalProvider, ServicePrincipalProvider>()
                .AddTransient<IAzureClientProvider, AzureClientClientProvider>();
        }
    }
}