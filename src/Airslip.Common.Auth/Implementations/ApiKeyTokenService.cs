using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
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

        public override NewToken GenerateNewToken(GenerateApiKeyToken token)
        {
            List<Claim> claims = new()
            {
                new Claim("correlation", Guid.NewGuid().ToString()),
                new Claim("apikeyusagetype", token.AirslipUserType.ToString()),
                new Claim("apikey", token.ApiKey),
                new Claim("entityid", token.EntityId),
                new Claim("ip", _remoteIpAddressService.GetRequestIP() ?? "UNKNOWN")
            };

            return GenerateNewToken(claims);
        }
        
        public override ApiKeyToken GetCurrentToken()
        {
            ClaimsPrincipal claimsPrincipal = _httpContext.User;

            return GenerateTokenFromClaims(claimsPrincipal.Claims, claimsPrincipal.Identity?.IsAuthenticated);
        }

        protected override ApiKeyToken GenerateTokenFromClaims(IEnumerable<Claim> tokenClaims, bool? isAuthenticated)
        {
            string correlationId = tokenClaims.GetValue("correlation");
            correlationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString() : correlationId;
            Log.Logger.ForContext(nameof(correlationId), correlationId);

            AirslipUserType airslipUserType;
            if (!Enum.TryParse(tokenClaims.GetValue("airslipUserType"), out airslipUserType))
            {
                airslipUserType = AirslipUserType.Merchant;
            }
            
            return new ApiKeyToken(
                isAuthenticated,
                tokenClaims.GetValue("apikey"),
                tokenClaims.GetValue("entityid"),
                airslipUserType,
                correlationId,
                tokenClaims.GetValue("ip"),
                _httpContext.Request.Headers["Authorization"]
            );
        }
    }
}