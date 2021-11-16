using Airslip.Common.Types.Interfaces;
using Airslip.Common.Utilities.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.Utilities.Extensions
{
    public static class HttpExtensions
    {
        public static async Task<HttpRequestResult<TResponse>> GetApiRequest<TResponse>(this HttpClient httpClient,  string url, 
            Dictionary<string, string> headers, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Get,
                RequestUri = new Uri(url)
            };
            
            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);                
            }

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }
        
        public static async Task<HttpRequestResult<TResponse>> GetApiRequest<TResponse>(this HttpClient httpClient,  string url, 
            string apiKey, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            return await GetApiRequest<TResponse>(httpClient, url, new Dictionary<string, string>
            {
                {"x-api-key", apiKey}
            }, cancellationToken);
        }
        
        public static async Task<HttpRequestResult<TResponse>> PatchApiRequest<TResponse>(this HttpClient httpClient,  string url, 
            Dictionary<string, string> headers, string requestContent, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Patch,
                RequestUri = new Uri(url),
                Content = new StringContent(requestContent, 
                    Encoding.UTF8, Json.MediaType)
            };
            
            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);                
            }

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }

        public static async Task<HttpRequestResult<TResponse>> PostApiRequest<TResponse, TRequestType>(this HttpClient httpClient,  string url, 
            Dictionary<string, string> headers, TRequestType requestContent, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            HttpRequestMessage httpRequestMessage = new()
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(url),
                Content = new StringContent(Json.Serialize(requestContent!), 
                    Encoding.UTF8, Json.MediaType)
            };
            
            foreach (KeyValuePair<string, string> keyValuePair in headers)
            {
                httpRequestMessage.Headers.Add(keyValuePair.Key, keyValuePair.Value);                
            }

            return await _sendRequest<TResponse>(httpClient, httpRequestMessage, cancellationToken);
        }
        
        public static async Task<HttpRequestResult<TResponse>> PostApiRequest<TResponse, TRequestType>(this HttpClient httpClient,  
            string url, string apiKey, TRequestType requestContent, CancellationToken cancellationToken)
            where TResponse : class, IResponse
        {
            return await PostApiRequest<TResponse, TRequestType>(httpClient, url, new Dictionary<string, string>
            {
                {"x-api-key", apiKey}
            }, requestContent, cancellationToken);
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