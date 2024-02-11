using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MobileTopUp.Web.Converter;
using MobileTopUp.Web.ExternalHttpClient;

namespace MobileTopUp.Web
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWeb(this IServiceCollection services)
        {

            services.AddHttpClient<BalanceHttpClientService>((provider, c) =>
            {
                c.Timeout = TimeSpan.FromMinutes(10);
                c.BaseAddress = new Uri("https://localhost:7076");
            });

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSingleton<IJsonConverter, JsonConverter>();
            return services;
        }
    }
}
