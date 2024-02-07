using IdentityModel.Client;

namespace MobileTopUp.Web.ExternalHttpClient
{
    public class HttpClientServiceMoq : IHttpClientService
    {
        public Task<string> GetBalanceAsync(string userId, CancellationToken cancellationToken)
        {
            Random random = new Random();
            long result = random.NextInt64(1, 5000);
            return Task.FromResult(result.ToString());
        }

        public Task<bool> DebitAsync(string userId, long amount, CancellationToken cancellationToken)
        {

            return Task.FromResult(true);
        }

        public Task<TokenResponse> RequestTokenAsync(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            return Task.FromResult(new TokenResponse());
        }
    }
}
