
using Xunit;

namespace MobileTopup.Api.IntegrationTests.Common.WebApplicationFactory
{

    [CollectionDefinition(CollectionName)]
    public class BalanceWebAppFactoryCollection : ICollectionFixture<BalanceWebAppFactory>
    {
        public const string CollectionName = "BalanceWebAppFactoryCollection";
    }
}
