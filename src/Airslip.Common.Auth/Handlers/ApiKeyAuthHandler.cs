using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Schemes;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Handlers
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly IApiKeyValidator _apiKeyValidator;

        public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, IApiKeyValidator apiKeyValidator) 
            : base(options, logger, encoder, clock)
        {
            _apiKeyValidator = apiKeyValidator;
        }

        protected override async Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey(ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField))
            {
                return AuthenticateResult.NoResult();
            }

            KeyValuePair<string, StringValues> apiKeyToken = Request
                .Headers
                .First(o => o.Key == ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField);
                
            ClaimsPrincipal? apiKeyPrincipal = await _apiKeyValidator.IsApiKeyTokenValid(apiKeyToken.Value.First());

            if (apiKeyPrincipal == null)
            {
                return AuthenticateResult.Fail("Api key invalid");
            }

            return AuthenticateResult.Success(new AuthenticationTicket(apiKeyPrincipal, ApiKeyAuthenticationSchemeOptions.ApiKeyScheme));
        }
    }
}