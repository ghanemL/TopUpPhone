using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace MobileTopup.Api.IntegrationTests.Common.WebApplicationFactory
{
    public class BalanceWebAppFactory : WebApplicationFactory<Balance.Api.IAssemblyMarker>, IAsyncLifetime
    {
        public BalanceHttpClient CreateBalanceHttpClient()
        {
            return new BalanceHttpClient(CreateClient());
        }

        public Task InitializeAsync() => Task.CompletedTask;

        public new Task DisposeAsync()
        {
     
            return Task.CompletedTask;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration((context, conf) => conf.AddInMemoryCollection(new Dictionary<string, string?>()));
        }
    }
}
