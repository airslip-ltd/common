using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Handlers
{
    public class ApiKeyAuthHandler : AuthenticationHandler<ApiKeyAuthenticationSchemeOptions>
    {
        private readonly ITokenValidator<ApiKeyToken, GenerateApiKeyToken> _tokenValidator;

        public ApiKeyAuthHandler(IOptionsMonitor<ApiKeyAuthenticationSchemeOptions> options, 
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock, ITokenValidator<ApiKeyToken, GenerateApiKeyToken> tokenValidator) 
            : base(options, logger, encoder, clock)
        {
            _tokenValidator = tokenValidator;
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

            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator
                    .GetClaimsPrincipalFromToken(apiKeyToken.Value.First(), ApiKeyAuthenticationSchemeOptions.ApiKeyScheme);

                return apiKeyPrincipal == null ? 
                    AuthenticateResult.Fail("Api key invalid") : 
                    AuthenticateResult.Success(new AuthenticationTicket(apiKeyPrincipal, ApiKeyAuthenticationSchemeOptions.ApiKeyScheme));
            }
            catch (ArgumentException)
            {
                return AuthenticateResult.Fail("Api key invalid");
            }
        }
    }
}