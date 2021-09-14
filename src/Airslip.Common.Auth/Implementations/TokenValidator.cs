using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Implementations
{
    public class TokenValidator<TExisting, TNew> : ITokenValidator<TExisting, TNew> 
        where TExisting : TokenBase 
        where TNew : GenerateTokenBase
    {
        private readonly ITokenService<TExisting, TNew> _tokenService;

        public TokenValidator(ITokenService<TExisting, TNew> tokenService)
        {
            _tokenService = tokenService;
        }
        
        public async Task<ClaimsPrincipal?> GetClaimsPrincipalFromToken(string value, string forScheme, 
            string forEnvironment)
        {
            Tuple<TExisting, ICollection<Claim>> tokenDetails = _tokenService.DecodeExistingToken(value);

            List<Claim> list = tokenDetails.Item2.ToList();
            
            if (list.All(o => o.Type != "Environment") || list.GetValue("Environment") != forEnvironment)
            {
                throw new EnvironmentUnsupportedException(list.GetValue("Environment"), 
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

    public class EnvironmentUnsupportedException : Exception
    {
        public string SuppliedEnvironment { get; }
        public string SupportedEnvironment { get; }

        public EnvironmentUnsupportedException(string suppliedEnvironment, string supportedEnvironment)
        {
            SuppliedEnvironment = suppliedEnvironment;
            SupportedEnvironment = supportedEnvironment;
        }
    }
}