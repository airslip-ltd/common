using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class ApiKeyValidator : IApiKeyValidator
    {
        private readonly ITokenService<ApiKeyToken, GenerateApiKeyToken> _tokenService;

        public ApiKeyValidator(ITokenService<ApiKeyToken, GenerateApiKeyToken> tokenService)
        {
            _tokenService = tokenService;
        }
        
        public async Task<ClaimsPrincipal?> GetClaimsPrincipalFromApiKeyToken(string value)
        {
            Tuple<ApiKeyToken, IEnumerable<Claim>> tokenDetails = _tokenService.DecodeExistingToken(value);
            
            List<ClaimsIdentity> claimsIdentities = new()
            {
                new ClaimsIdentity(tokenDetails.Item2, ApiKeyAuthenticationSchemeOptions.ApiKeyScheme)
            };
            
            ClaimsPrincipal principal = new(claimsIdentities);

            return await Task.FromResult(principal);
        }
    }
}