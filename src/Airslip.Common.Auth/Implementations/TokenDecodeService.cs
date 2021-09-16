using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Types;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Airslip.Common.Auth.Implementations
{
    public class TokenDecodeService<TTokenType> : ITokenDecodeService<TTokenType> 
        where TTokenType : IDecodeToken, new()
    {
        private readonly IHttpHeaderLocator _httpHeaderLocator;
        private readonly IClaimsPrincipalLocator _claimsPrincipalLocator;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenDecodeService(IHttpHeaderLocator httpHeaderLocator, IClaimsPrincipalLocator claimsPrincipalLocator)
        {
            _httpHeaderLocator = httpHeaderLocator;
            _claimsPrincipalLocator = claimsPrincipalLocator;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }
        
        public Tuple<TTokenType, ICollection<Claim>> DecodeExistingToken(string tokenValue)
        {
            try
            {
                JwtSecurityToken token = _jwtSecurityTokenHandler.ReadJwtToken(tokenValue);

                return new Tuple<TTokenType, ICollection<Claim>>(GenerateTokenFromClaims(token.Claims.ToList(), true),
                    token.Claims.ToList());
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Token is not in expected format", nameof(tokenValue));
            }
        }

        public TTokenType GetCurrentToken()
        {
            ClaimsPrincipal? claimsPrincipal = _claimsPrincipalLocator.GetCurrentPrincipal();

            List<Claim> claims = claimsPrincipal?.Claims.ToList() ?? new List<Claim>();
            
            return GenerateTokenFromClaims(claims, claimsPrincipal?.Identity?.IsAuthenticated ?? false);
        }

        public TTokenType GenerateTokenFromClaims(ICollection<Claim> tokenClaims, bool? isAuthenticated)
        {
            
            string correlationId = tokenClaims.GetValue(AirslipClaimTypes.CORRELATION);
            if (!Enum.TryParse(tokenClaims.GetValue(AirslipClaimTypes.AIRSLIP_USER_TYPE), out AirslipUserType airslipUserType))
            {
                airslipUserType = AirslipUserType.Merchant;
            }

            TTokenType result = new()
            {
                CorrelationId = string.IsNullOrWhiteSpace(correlationId) ? CommonFunctions.GetId() : correlationId,
                AirslipUserType = airslipUserType,
                IsAuthenticated = isAuthenticated,
                BearerToken = _httpHeaderLocator.GetValue("Authorization") ?? "",
                TokenType = nameof(TTokenType),
                IpAddress = tokenClaims.GetValue(AirslipClaimTypes.IP_ADDRESS),
                EntityId = tokenClaims.GetValue(AirslipClaimTypes.ENTITY_ID),
                Environment = tokenClaims.GetValue(AirslipClaimTypes.ENVIRONMENT),
                UserAgent = tokenClaims.GetValue(AirslipClaimTypes.USER_AGENT) 
            };

            result
                .SetCustomClaims(tokenClaims.ToList());

            return result;
        }
    }
}