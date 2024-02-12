﻿using IdentityModel.Client;
using MobileTopUp.Web.Converter;
using System.Net.Http.Json;
using System.Text;

namespace MobileTopUp.Web.ExternalHttpClient
{
    public class BalanceHttpClientService  : IBalanceHttpClientService
    {
        private HttpClient _httpClient;
        private readonly IJsonConverter _jsonConverter;

        public BalanceHttpClientService(
           HttpClient httpClient,
           IJsonConverter jsonConverter)
        {
            _httpClient = httpClient;
            _jsonConverter = jsonConverter;
        }

        public async Task<string> GetBalanceAsync(string userId, CancellationToken cancellationToken)
        {
            var message = await GetRequestAsync(new Uri("https://localhost:7076/getBalance"), cancellationToken);
            var response = await _httpClient.GetAsync(message.RequestUri, cancellationToken).ConfigureAwait(false);
            return await response.Content.ReadAsStringAsync().ConfigureAwait(false);
        }

        public async Task<bool>  DebitAsync(string userId, long amount, CancellationToken cancellationToken)
        {
            var message =  await _httpClient.PostAsJsonAsync($"https://localhost:7076/executeDebit", userId).ConfigureAwait(false);
            return message.IsSuccessStatusCode;
        }

        public async Task<T> SendAndDeserializeJsonAsync<T>(HttpRequestMessage message, CancellationToken cancellationToken)
        {
            var response = await _httpClient.SendAsync(message, cancellationToken).ConfigureAwait(false);
            var responseContent = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);

            return _jsonConverter.Deserialize<T>(responseContent);
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

        private async Task<HttpRequestMessage> CreatePostRequestAsync(Uri method, CancellationToken cancellationToken)
        {
            var message = new HttpRequestMessage(HttpMethod.Post, method)
            {
                Content = new StringContent(string.Empty, Encoding.UTF32, "application/json")
            };

            //message.Headers.Authorization = new AuthenticationHeaderValue("Bearer", await GetTokenAsync("scope", cancellationToken));
            return message;
        }
    }
}