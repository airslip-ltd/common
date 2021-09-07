using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Airslip.Common.Auth.Implementations
{
    public class ApiKeyTokenService : TokenService<ApiKeyToken, GenerateApiKeyToken>
    {
        private readonly IRemoteIpAddressService _remoteIpAddressService;
        private readonly HttpContext _httpContext;

        public ApiKeyTokenService(IHttpContextAccessor httpContextAccessor, 
            IRemoteIpAddressService remoteIpAddressService, IOptions<JwtSettings> jwtSettings)
        : base(jwtSettings)
        {
            _remoteIpAddressService = remoteIpAddressService;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public override string GenerateNewToken(GenerateApiKeyToken token, DateTime? expiresTime = null)
        {
            List<Claim> claims = new()
            {
                new Claim("correlation", Guid.NewGuid().ToString()),
                new Claim("apikeyusagetype", token.ApiKeyUsageType.ToString()),
                new Claim("apikey", token.ApiKey),
                new Claim("entityid", token.EntityId),
                new Claim("ip", _remoteIpAddressService.GetRequestIP() ?? "UNKNOWN")
            };

            return GenerateNewToken(claims, expiresTime);
        }
        
        public override ApiKeyToken GetCurrentToken()
        {
            ClaimsPrincipal claimsPrincipal = _httpContext.User;

            return generateTokenFromClaims(claimsPrincipal.Claims, claimsPrincipal.Identity?.IsAuthenticated);
        }

        public override Tuple<ApiKeyToken, IEnumerable<Claim>> DecodeExistingToken(string tokenValue)
        {
            JwtSecurityToken token = _jwtSecurityTokenHandler.ReadJwtToken(tokenValue);

            return new Tuple<ApiKeyToken, IEnumerable<Claim>>(generateTokenFromClaims(token.Claims, true),
                token.Claims);
        }

        private ApiKeyToken generateTokenFromClaims(IEnumerable<Claim> claims, bool? isAuthenticated)
        {
            string correlationId = claims.GetValue("correlation");
            correlationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString() : correlationId;
            Log.Logger.ForContext(nameof(correlationId), correlationId);

            ApiKeyUsageType apiKeyUsageType;
            if (!Enum.TryParse(claims.GetValue("apikeyusagetype"), out apiKeyUsageType))
            {
                apiKeyUsageType = ApiKeyUsageType.Merchant;
            }
            
            return new ApiKeyToken(
                isAuthenticated,
                claims.GetValue("apikey"),
                claims.GetValue("entityid"),
                apiKeyUsageType,
                correlationId,
                claims.GetValue("ip"),
                _httpContext.Request.Headers["Authorization"]
            );
        }
    }
}