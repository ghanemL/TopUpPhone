using Microsoft.Extensions.DependencyInjection;
using MobileTopUp.Web.ExternalHttpClient;

namespace MobileTopUp.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {
            services.AddScoped<IHttpClientService, HttpClientServiceMoq>();
            return services;
        }
    }
}
