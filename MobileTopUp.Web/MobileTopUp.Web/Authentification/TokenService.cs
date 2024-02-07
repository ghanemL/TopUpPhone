using Microsoft.Extensions.Logging;
using IdentityModel.Client;
using MobileTopUp.Web.Converter;
using MobileTopUp.Web.ExternalHttpClient;

namespace MobileTopUp.Web.Authentification
{
    public class TokenHttpClientService : HttpClientService
    {
        public TokenHttpClientService(HttpClient httpClient, ILogger<TokenHttpClientService> logger, IJsonConverter jsonConverter)
            : base(httpClient, logger, jsonConverter)
        {
        }
    }

    public class TokenService
    {
        private readonly TokenHttpClientService _httpClient;
        protected readonly ILogger<TokenService> Logger;
        private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1, 1);
        public TokenService(TokenHttpClientService httpClient, ILogger<TokenService> logger)
        {
            _httpClient = httpClient;
            Logger = logger;
        }

        public virtual async Task<string> GetTokenAsync(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            return (await GetTokenResponseAsync(tokenRequest, cancellationToken)).AccessToken;
        }
        protected async Task<TokenResponse> GetTokenResponseAsync(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            await Semaphore.WaitAsync(cancellationToken);
            TokenResponse oktaResponse;
            try
            {
                oktaResponse = await _httpClient.RequestTokenAsync(tokenRequest, cancellationToken);
            }
            finally
            {
                Semaphore.Release();
            }
            Logger.LogDebug(
                    $"{GetType().Name} - Finished getting Okta token with code : {oktaResponse?.HttpStatusCode}.");
            if (oktaResponse == null || oktaResponse.IsError)
            {
                Logger.LogError(
                    $"{GetType().Name} - Okta Error : {oktaResponse?.Error} : {oktaResponse?.ErrorDescription}.");
                throw new Exception(
                    $"unable to retreive token for {tokenRequest?.ClientId} on {tokenRequest?.Address} with error {oktaResponse?.ErrorDescription}");
            }
            return oktaResponse;
        }
    }
}
