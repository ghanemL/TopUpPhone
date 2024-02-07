using IdentityModel.Client;

namespace MobileTopUp.Web.ExternalHttpClient
{
    public interface IHttpClientService
    {
        Task<string> GetBalanceAsync(string userId, CancellationToken cancellationToken);
        Task<bool> DebitAsync(string userId, long amount, CancellationToken cancellationToken);
        Task<TokenResponse> RequestTokenAsync(TokenRequest tokenRequest, CancellationToken cancellationToken);

    }
}
