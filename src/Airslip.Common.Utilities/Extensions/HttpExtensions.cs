using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.Utilities.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<HttpRequestResult<TResponse>> GetApiRequest<TResponse>(this HttpClient httpClient,  string url, 
            string apiKey, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url),
                Headers = { 
                    { "x-api-key", apiKey }
                }
            };

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }
        
        public static async Task<HttpRequestResult<TResponse>> PostApiRequest<TResponse, TRequestType>(
            this HttpClient httpClient,  
            string url,
            string apiKey,
            TRequestType requestContent, 
            CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Headers = { 
                    { "x-api-key", apiKey }
                },
                Content = new StringContent(Json.Serialize(requestContent!), 
                    Encoding.UTF8, Json.MediaType)
            };

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }
        
        public static async Task<HttpRequestResult<TResponse>> PostApiRequest<TResponse, TRequestType>(
            this HttpClient httpClient,
            string url,
            IDictionary<string, string> requestHeaders,
            TRequestType requestContent,
            CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new StringContent(
                    Json.Serialize(requestContent!),
                    Encoding.UTF8, 
                    Json.MediaType)
            };

            foreach (KeyValuePair<string, string> requestHeader in requestHeaders)
            {
                var (key, value) = requestHeader;
                httpRequestMessage.Headers.Add(key, value);
            }

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }

        private static async Task<HttpRequestResult<TResponse>> _sendRequest<TResponse>(HttpClient httpClient, 
            HttpRequestMessage httpRequestMessage, CancellationToken cancellationToken) 
            where TResponse : class, IResponse
        {
            HttpResponseMessage response = await httpClient.SendAsync(httpRequestMessage, cancellationToken);

            string content = await response.Content.ReadAsStringAsync(cancellationToken);

            if (response.IsSuccessStatusCode)
            {
                return new HttpRequestResult<TResponse>(response.IsSuccessStatusCode, response.StatusCode, content, 
                    ValidContent(content) ? Json.Deserialize<TResponse>(content) : null);
            }

            return new HttpRequestResult<TResponse>(response.IsSuccessStatusCode, response.StatusCode, content);
        }

        private static bool ValidContent(string content)
        { 
            return content == string.Empty;
        }
    }
}