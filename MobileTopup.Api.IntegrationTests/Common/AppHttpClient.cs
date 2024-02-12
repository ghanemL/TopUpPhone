using FluentAssertions;
using MobileTopup.Application.Options.Queries;
using MobileTopup.Application.Topups.Queries.GetTopUpBeneficiaries;
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

        public async Task<UserResponse> AddBeneficiaryExpectSuccessAsync(AddTopUpBeneficiaryRequest addBeneficiaryRequest)
        {
            var response = await AddBeneficiaryAsync(addBeneficiaryRequest);

            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var userResponse = await response.Content.ReadFromJsonAsync<UserResponse>();

            userResponse.Should().NotBeNull();

            return userResponse!;
        }

        public async Task<HttpResponseMessage> AddBeneficiaryExpectErrorAsync(AddTopUpBeneficiaryRequest addBeneficiaryRequest)
        {
            var response = await AddBeneficiaryAsync(addBeneficiaryRequest);


            response.IsSuccessStatusCode.Should().BeFalse();

            return response;
        }


        public async Task<HttpResponseMessage> CreateUserAsync(CreateUserRequest? createUserRequest)
        {
            createUserRequest ??= new CreateUserRequest { Name = "User1" };
            return await _httpClient.PostAsJsonAsync($"api/TopUp/createUser", createUserRequest).ConfigureAwait(false); ;
        }

        public async Task<TopUpOptionResponse> GetAvailableTopUpOptionsAndExpectSuccessAsync()
        {
            var response = await GetAvailableTopUpOptionsAsync();

            response.Should().NotBeNull();

            return response!;
        }

        public async Task<TopUpOptionResponse> GetAvailableTopUpOptionsAsync()
        {
            return await _httpClient.GetFromJsonAsync<TopUpOptionResponse>($"api/TopUp/getAvailableTopUpOptions").ConfigureAwait(false); ;
            
        }

        public async Task<HttpResponseMessage> AddBeneficiaryAsync(AddTopUpBeneficiaryRequest addBeneficiaryRequest)
        {
            return await _httpClient.PostAsJsonAsync($"api/TopUp/addBeneficiary", addBeneficiaryRequest).ConfigureAwait(false);
        }

        public async Task<List<TopUpBeneficiaryResponse>> GetBeneficiariesAndExpectSuccessAsync(GetTopUpBeneficiariesQuery getBeneficiariesRequest)
        {
            return await _httpClient.GetFromJsonAsync<List<TopUpBeneficiaryResponse>>($"api/TopUp/getBeneficiaries?userId={getBeneficiariesRequest.UserId}").ConfigureAwait(false);
        }

        public async Task<HttpResponseMessage> GetBeneficiariesAndExpectErrorAsync(GetTopUpBeneficiariesQuery getBeneficiariesRequest)
        {
            return await _httpClient.GetAsync($"api/TopUp/getBeneficiaries?userId={getBeneficiariesRequest.UserId}").ConfigureAwait(false);
        }

        internal async Task<HttpResponseMessage> ExecuteTopUpAsync(ExecuteTopUpRequest executeTopUpRequest)
        {
            return await _httpClient.PostAsJsonAsync($"api/TopUp/executeTopUp", executeTopUpRequest).ConfigureAwait(false);
        }
    }
}
