using IdentityModel.Client;
using Microsoft.Extensions.Logging;
using MobileTopUp.Web.Converter;
using System.Text;

namespace MobileTopUp.Web.ExternalHttpClient
{
    public class HttpClientService : IHttpClientService
    {
        private HttpClient _httpClient;
        protected ILogger Logger;
        private readonly IJsonConverter _jsonConverter;

        protected HttpClientService(
           HttpClient httpClient,
           ILogger logger,
           IJsonConverter jsonConverter)
        {
            _httpClient = httpClient;
            _jsonConverter = jsonConverter;
        }

        public async Task<string> GetBalanceAsync(string userId, CancellationToken cancellationToken)
        {
            var message = await GetRequestAsync(new Uri("www.external-api.com"), cancellationToken);
            var response = await _httpClient.GetAsync(message.RequestUri, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogWarning($"Call of {message.RequestUri} response code : {response.StatusCode}. Content:");
                try
                {
                    Logger.LogWarning($"{await response.Content.ReadAsStringAsync().ConfigureAwait(false)}");
                }
                catch (Exception e)
                {
                    Logger.LogWarning(e, $"Error displaying content of {message.RequestUri}");
                }
                response.EnsureSuccessStatusCode();
            }
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<bool>  DebitAsync(string userId, long amount, CancellationToken cancellationToken)
        {
            var requestId = Guid.NewGuid();
            var message = await CreatePostRequestAsync(new Uri("www.externalapi.com"), string.Format(userId, amount), cancellationToken);
            var result = await SendAndDeserializeJsonAsync<bool>(message, cancellationToken);
            return result;
        }

        public async Task<T> SendAndDeserializeJsonAsync<T>(HttpRequestMessage message, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
            if (!response.IsSuccessStatusCode)
            {
                Logger.LogWarning($"Call of {message.RequestUri} response code : {response.StatusCode}. Content:");
                try
                {
                    Logger.LogWarning($"{await response.Content.ReadAsStringAsync().ConfigureAwait(false)}");
                }
                catch (Exception e)
                {
                    Logger.LogWarning(e, $"Error displaying content of {message.RequestUri}");
                }
                response.EnsureSuccessStatusCode();
            }
            var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            try
            {
                return _jsonConverter.Deserialize<T>(responseContent);
            }
            catch (Exception e)
            {
                Logger.LogWarning(e, $"Unable to deserialize response of {message.RequestUri}. Content:\n{responseContent}");
                throw;
            }
        }


        private async Task<HttpRequestMessage> GetRequestAsync(Uri method, CancellationToken cancellationToken)
        {
            var message = new HttpRequestMessage(HttpMethod.Get, method);
            //message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync("scope", cancellationToken));
            return message;
        }

        public async Task<TokenResponse> RequestTokenAsync(TokenRequest tokenRequest, CancellationToken cancellationToken)
        {
            return await _httpClient.RequestTokenAsync(tokenRequest, cancellationToken);
        }

        private async Task<HttpRequestMessage> CreatePostRequestAsync(Uri method, string content, CancellationToken cancellationToken)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, method)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")
            };
            //message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync("scope", cancellationToken));
            return message;
        }
    }
}