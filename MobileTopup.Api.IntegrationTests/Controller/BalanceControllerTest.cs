using FluentAssertions;
using MobileTopup.Api.IntegrationTests.Common;
using MobileTopup.Api.IntegrationTests.Common.WebApplicationFactory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace MobileTopup.Api.IntegrationTests.Controller
{
    [Collection(BalanceWebAppFactoryCollection.CollectionName)]
    public class BalanceControllerTest
    {
        private readonly BalanceHttpClient _client;
        public BalanceControllerTest(BalanceWebAppFactory webAppFactory)
        {
            _client = webAppFactory.CreateBalanceHttpClient();

        }

        [Fact]
        public async Task GetBalance_ShouldSuccess()
        {
            var response = await _client.GetBalanceAndExpectSuccessAsync(Guid.NewGuid());

            response.Should().BePositive();
        }
    }
}
