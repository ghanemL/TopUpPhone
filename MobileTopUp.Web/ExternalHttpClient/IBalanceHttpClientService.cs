namespace MobileTopUp.Web.ExternalHttpClient
{
    public interface IBalanceHttpClientService
    {
        Task<string> GetBalanceAsync(string userId, CancellationToken cancellationToken);
        Task<bool> DebitAsync(string userId, long amount, CancellationToken cancellationToken);
    }
}