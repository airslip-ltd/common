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

        public string GenerateNewToken(ICollection<Claim> claims)
        {
            var signingCredentials = getSigningCredentials();

            JwtSecurityToken token = new(
                _jwtSettings.Issuer,
                _jwtSettings.Audience,
                claims,
                expires: _jwtSettings.ExpiresTime > 0 ? DateTime.Now.AddSeconds(_jwtSettings.ExpiresTime) : null,
                signingCredentials: signingCredentials);

            return _jwtSecurityTokenHandler.WriteToken(token);
        }
        
        public Tuple<TTokenType, IEnumerable<Claim>>? DecodeExistingToken(string tokenValue)
        {
            try
            {
                JwtSecurityToken token = _jwtSecurityTokenHandler.ReadJwtToken(tokenValue);

                return new Tuple<TTokenType, IEnumerable<Claim>>(GenerateTokenFromClaims(token.Claims, true),
                    token.Claims);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("Token is not in expected format", nameof(tokenValue));
            }
        }

        public abstract string GenerateNewToken(TGenerateTokenType token);

        public abstract TTokenType GetCurrentToken();

        protected abstract TTokenType GenerateTokenFromClaims(IEnumerable<Claim> tokenClaims, bool? isAuthenticated);

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