using Airslip.Common.Auth.Data;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.MerchantTransactions.Common.Data
{
    public abstract class MerchantIntegrationApi : IMerchantIntegrationApi
    {
        public string ApiKeyToken { get; private set; } = String.Empty;

        public void SetApiKeyToken(string token)
        {
            ApiKeyToken = token;
        }

        // Called by implementing swagger client classes
        protected Task<HttpRequestMessage> CreateHttpRequestMessageAsync(CancellationToken cancellationToken)
        {
            var msg = new HttpRequestMessage();
            msg.Headers.Add(AirslipSchemeOptions.ApiKeyHeaderField, ApiKeyToken);
            return Task.FromResult(msg);
        }
    }
}