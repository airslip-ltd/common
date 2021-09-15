using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Handlers
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly IApiKeyRequestHandler _apiKeyRequestHandler;

        public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyRequestHandler apiKeyRequestHandler) 
            : base(options, logger, encoder, clock)
        {
            _apiKeyRequestHandler = apiKeyRequestHandler;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            return await _apiKeyRequestHandler.Handle(Request);
        }
    }
}