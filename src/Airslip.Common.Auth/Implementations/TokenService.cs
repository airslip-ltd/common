using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Airslip.Common.Auth.Implementations
{
    public abstract class TokenService<TTokenType, TGenerateTokenType> : ITokenService<TTokenType, TGenerateTokenType> 
        where TTokenType : TokenBase
        where TGenerateTokenType : GenerateTokenBase
    {
        private readonly JwtSettings _jwtSettings;
        protected readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler;

        public TokenService(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
            _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        }

        public string GenerateNewToken(ICollection<Claim> claims, DateTime? expiresTime = null)
        {
            var signingCredentials = getSigningCredentials();

            JwtSecurityToken token = new(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: expiresTime,
                signingCredentials: signingCredentials);

            return _jwtSecurityTokenHandler.WriteToken(token);
        }

        public abstract string GenerateNewToken(TGenerateTokenType token, DateTime? expiresTime = null);

        public abstract TTokenType GetCurrentToken();

        public abstract Tuple<TTokenType, IEnumerable<Claim>> DecodeExistingToken(string tokenValue);

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