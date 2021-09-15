using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class ApiKeyRequestHandler : IApiKeyRequestDataHandler
    {
        private readonly ITokenValidator<ApiKeyToken> _tokenValidator;

        public ApiKeyRequestHandler(ITokenValidator<ApiKeyToken> tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        
        public async Task<KeyAuthenticationResult> Handle(HttpRequestData request)
        {
            if (!request.Headers.Contains(ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField))
            {
                return KeyAuthenticationResult.Fail($"{ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField} header not found");
            }

            List<string> headerValue = request
                .Headers.GetValues(ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField)
                .ToList();

            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator
                    .GetClaimsPrincipalFromToken(headerValue.First(), 
                        ApiKeyAuthenticationSchemeOptions.ApiKeyScheme, 
                        ApiKeyAuthenticationSchemeOptions.ThisEnvironment);

                return apiKeyPrincipal == null ? 
                    KeyAuthenticationResult.Fail("Api key invalid") : 
                    KeyAuthenticationResult.Valid();
            }
            catch (ArgumentException)
            {
                return KeyAuthenticationResult.Fail("Api key invalid");
            }
            catch (EnvironmentUnsupportedException)
            {
                return KeyAuthenticationResult.Fail("Environment Unsupported");
            }
        }
    }
}