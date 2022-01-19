using Airslip.Common.Auth.Data;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.MerchantTransactions
{
    public abstract class MerchantIntegrationApi : IMerchantIntegrationApi
    {
        private string ApiKeyToken { get; set; } = String.Empty;

        public void SetApiKeyToken(string token)
        {
            ApiKeyToken = token;
        }

        // Called by implementing swagger client classes
        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            HttpRequestMessage msg = new();
            msg.Headers.Add(AirslipSchemeOptions.ApiKeyHeaderField, ApiKeyToken);
            return Task.FromResult(msg);
        }
    }
}