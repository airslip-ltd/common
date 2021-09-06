using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;

namespace Airslip.Common.Auth.Implementations
{
    public class UserTokenService : TokenService<UserToken, GenerateUserToken>
    {
        private readonly IUserAgentService _userAgentService;
        private readonly IRemoteIpAddressService _remoteIpAddressService;
        private readonly HttpContext _httpContext;

        public UserTokenService(IHttpContextAccessor httpContextAccessor, 
            IUserAgentService userAgentService,
            IRemoteIpAddressService remoteIpAddressService, 
            IOptions<JwtSettings> jwtSettings)
        : base(jwtSettings)
        {
            _userAgentService = userAgentService;
            _remoteIpAddressService = remoteIpAddressService;
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public override string GenerateNewToken(GenerateUserToken token, DateTime? expiresTime = null)
        {
            List<Claim> claims = new()
            {
                new Claim("correlation", Guid.NewGuid().ToString()),
                new Claim("userid", token.UserId),
                new Claim("yapilyuserid", token.YapilyUserId),
                new Claim("identity", token.Identity),
                new Claim("ip", _remoteIpAddressService.GetRequestIP() ?? "UNKNOWN"),
                new Claim("ua", _userAgentService.GetRequestUserAgent() ?? "UNKNOWN")
            };

            return GenerateNewToken(claims, expiresTime);
        }

        public override UserToken GetCurrentToken()
        {
            ClaimsPrincipal claimsPrincipal = _httpContext.User;
            List<Claim> claims = claimsPrincipal.Claims.ToList(); 

            return generateTokenFromClaims(claims, claimsPrincipal.Identity?.IsAuthenticated);
        }

        public override Tuple<UserToken, IEnumerable<Claim>> DecodeExistingToken(string tokenValue)
        {
            JwtSecurityToken token = _jwtSecurityTokenHandler.ReadJwtToken(tokenValue);

            return new Tuple<UserToken, IEnumerable<Claim>>(generateTokenFromClaims(token.Claims, true),
                token.Claims);
        }
        
        private UserToken generateTokenFromClaims(IEnumerable<Claim> claims, bool? isAuthenticated)
        {
            string correlationId = claims.GetValue("correlation");
            correlationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString() : correlationId;
            Log.Logger.ForContext(nameof(correlationId), correlationId);

            return new UserToken(
                isAuthenticated,
                claims.GetValue("userid"),
                claims.GetValue("yapilyuserid"),
                claims.GetValue("identity"),
                correlationId,
                claims.GetValue("ip"),
                claims.GetValue("ua"),
                _httpContext.Request.Headers["Authorization"]
            );
        }
    }
}