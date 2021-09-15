using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Airslip.Common.Types;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Airslip.Common.Auth.Implementations
{
    public class TokenGenerationService<TGenerateTokenType> : ITokenGenerationService<TGenerateTokenType> 
        where TGenerateTokenType : IGenerateToken
    {
        private readonly IRemoteIpAddressService _remoteIpAddressService;
        private readonly IUserAgentService _userAgentService;
        private readonly JwtSettings _jwtSettings;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenGenerationService(IOptions<JwtSettings> jwtSettings, 
            IRemoteIpAddressService remoteIpAddressService,
            IUserAgentService userAgentService)
        {
            _remoteIpAddressService = remoteIpAddressService;
            _userAgentService = userAgentService;
            _jwtSettings = jwtSettings.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public NewToken GenerateNewToken(ICollection<Claim> claims)
        {
            SigningCredentials signingCredentials = getSigningCredentials();

            DateTime? expiryDate =
                _jwtSettings.ExpiresTime > 0 ? DateTime.Now.AddSeconds(_jwtSettings.ExpiresTime) : null;
            
            JwtSecurityToken token = new(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expiryDate,
                signingCredentials: signingCredentials);

            return new NewToken(_jwtSecurityTokenHandler.WriteToken(token), expiryDate);
        }

        public NewToken GenerateNewToken(TGenerateTokenType token)
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.CORRELATION, CommonFunctions.GetId()),
                new Claim(AirslipClaimTypes.AIRSLIP_USER_TYPE, token.AirslipUserType.ToString()),
                new Claim(AirslipClaimTypes.ENTITY_ID, token.EntityId),
                new Claim(AirslipClaimTypes.ENVIRONMENT, ApiKeyAuthenticationSchemeOptions.ThisEnvironment),
                new Claim(AirslipClaimTypes.IP_ADDRESS, _remoteIpAddressService.GetRequestIP() ?? "UNKNOWN"),
                new Claim(AirslipClaimTypes.USER_AGENT, _userAgentService.GetRequestUserAgent() ?? "UNKNOWN")
            };

            claims.AddRange(token.GetCustomClaims());

            return GenerateNewToken(claims);
        }
        
        private SigningCredentials getSigningCredentials()
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.Key))
                throw new ArgumentNullException(nameof(_jwtSettings.Key), "private key must be set");

            SymmetricSecurityKey key = new(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            if (key.KeySize < 256)
                throw new ArgumentException(
                    "the key must be at least 256 in length -> 32 characters in length at least",
                    nameof(_jwtSettings.Key));

            return new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        }
    }
}