using FluentAssertions;
using MobileTopup.Api.IntegrationTests.Common;
using MobileTopup.Api.IntegrationTests.Common.WebApplicationFactory;
using MobileTopup.Contracts.Requests;
using Xunit;

namespace MobileTopup.Api.IntegrationTests.Controller
{
    [Collection(WebAppFactoryCollection.CollectionName)]
    public class CreateUserTests
    {
        private readonly AppHttpClient _client;

        public CreateUserTests(WebAppFactory webAppFactory)
        {
            _client = webAppFactory.CreateAppHttpClient();
            webAppFactory.ResetDatabase();
        }

        [Fact]
        public async Task CreateUser_ShouldSuccess()
        {
            var expectedName = "UserTest1";

            var createUserRequest = new CreateUserRequest() { Name = expectedName };

            var response = await _client.CreateUserAndExpectSuccessAsync(createUserRequest: createUserRequest);

            response.Name.Should().Be(expectedName);
        }
    }
}
