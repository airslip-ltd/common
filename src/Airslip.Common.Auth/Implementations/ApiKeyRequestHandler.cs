using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class ApiKeyRequestHandler : IApiKeyRequestHandler
    {
        private readonly ITokenValidator<ApiKeyToken, GenerateApiKeyToken> _tokenValidator;

        public ApiKeyRequestHandler(ITokenValidator<ApiKeyToken, GenerateApiKeyToken> tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        
        public async Task<AuthenticateResult> Handle(HttpRequest request)
        {
            if (!request.Headers.ContainsKey(ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField))
            {
                return AuthenticateResult.NoResult();
            }

            KeyValuePair<string, StringValues> apiKeyToken = request
                .Headers
                .First(o => o.Key == ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField);

            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator
                    .GetClaimsPrincipalFromToken(apiKeyToken.Value.First(), 
                        ApiKeyAuthenticationSchemeOptions.ApiKeyScheme, 
                        ApiKeyAuthenticationSchemeOptions.ThisEnvironment);

                return apiKeyPrincipal == null ? 
                    AuthenticateResult.Fail("Api key invalid") : 
                    AuthenticateResult.Success(new AuthenticationTicket(apiKeyPrincipal, ApiKeyAuthenticationSchemeOptions.ApiKeyScheme));
            }
            catch (ArgumentException)
            {
                return AuthenticateResult.Fail("Api key invalid");
            }
            catch (EnvironmentUnsupportedException)
            {
                return AuthenticateResult.Fail("Environment Unsupported");
            }
        }
    }
}