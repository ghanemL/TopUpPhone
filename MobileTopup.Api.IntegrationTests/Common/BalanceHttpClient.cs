using System.Net.Http.Json;

namespace MobileTopup.Api.IntegrationTests.Common
{
    public class BalanceHttpClient
    {
        private readonly HttpClient _httpClient;

        public BalanceHttpClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<long> GetBalanceAndExpectSuccessAsync(Guid userId)
        {
            return await _httpClient.GetFromJsonAsync<long>($"/getBalance?userId={userId}").ConfigureAwait(false); ;
        }
    }
}
