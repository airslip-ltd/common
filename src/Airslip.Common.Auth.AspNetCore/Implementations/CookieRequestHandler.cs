using Airslip.Common.Auth.AspNetCore.Interfaces;
using Airslip.Common.Auth.AspNetCore.Schemes;
using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.AspNetCore.Implementations
{
    public class CookieRequestHandler : ICookieRequestHandler
    {
        private readonly ITokenValidator<UserToken> _tokenValidator;

        public CookieRequestHandler(ITokenValidator<UserToken> tokenValidator)
        {
            _tokenValidator = tokenValidator;
        }
        
        public async Task<KeyAuthenticationResult> Handle(HttpRequest request)
        {
            // Find and validate the cookie value
            if (!request.Cookies.ContainsKey(CookieSchemeOptions.CookieTokenField))
            {
                return KeyAuthenticationResult
                    .NoResult();
            }

            // Read and decode the Cookie
            
            
            KeyValuePair<string, StringValues> apiKeyToken = request
                .Headers
                .First(o => o.Key == AirslipSchemeOptions.ApiKeyHeaderField);

            try
            {
                ClaimsPrincipal? apiKeyPrincipal = await _tokenValidator
                    .GetClaimsPrincipalFromToken(apiKeyToken.Value.First(), 
                        AirslipSchemeOptions.ApiKeyScheme, 
                        AirslipSchemeOptions.ThisEnvironment);

                return apiKeyPrincipal == null
                    ? KeyAuthenticationResult.Fail("Api key invalid")
                    : KeyAuthenticationResult.Valid(apiKeyPrincipal);
                
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