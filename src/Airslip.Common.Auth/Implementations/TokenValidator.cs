using Airslip.Common.Auth.Exceptions;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class TokenValidator<TExisting> : ITokenValidator<TExisting> 
        where TExisting : IDecodeToken, new()
    {
        private readonly ITokenDecodeService<TExisting> _tokenService;

        public TokenValidator(ITokenDecodeService<TExisting> tokenService)
        {
            _tokenService = tokenService;
        }
        
        public async Task<ClaimsPrincipal?> GetClaimsPrincipalFromToken(string value, string forScheme, 
            string forEnvironment)
        {
            Tuple<TExisting, ICollection<Claim>> tokenDetails = _tokenService.DecodeExistingToken(value);

            List<Claim> list = tokenDetails.Item2.ToList();
            
            if (list.All(o => o.Type != "environment") || list.GetValue("environment") != forEnvironment)
            {
                throw new EnvironmentUnsupportedException(list.GetValue("environment"), 
                    forEnvironment);
            }

            List<ClaimsIdentity> claimsIdentities = new()
            {
                new ClaimsIdentity(tokenDetails.Item2, forScheme)
            };
            
            ClaimsPrincipal principal = new(claimsIdentities);
            
            return await Task.FromResult(principal);
        }
    }
}