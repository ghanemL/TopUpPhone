using FluentAssertions;
using MobileTopup.Contracts.Requests;
using MobileTopup.Contracts.Responses;
using System.Net;
using System.Net.Http.Json;

namespace MobileTopup.Api.IntegrationTests.Common
{
    public class AppHttpClient
    {
        private readonly HttpClient _httpClient;

        public AppHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<UserResponse> CreateUserAndExpectSuccessAsync(CreateUserRequest? createUserRequest = null)
        {
            var response = await CreateUserAsync(createUserRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();

            userResponse.Should().NotBeNull();

            return userResponse!;
        }

        public async Task<HttpResponseMessage> CreateUserAsync(CreateUserRequest? createUserRequest)
        {
            createUserRequest ??= new CreateUserRequest { Name = "User1" };
            return await _httpClient.PostAsJsonAsync($"api/TopUp/createUser", createUserRequest);
        }
    }
}
