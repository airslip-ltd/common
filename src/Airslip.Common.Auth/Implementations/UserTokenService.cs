using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Types;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Serilog;
using System;
using System.Collections.Generic;
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

        public override NewToken GenerateNewToken(GenerateUserToken token)
        {
            List<Claim> claims = new()
            {
                new Claim("correlation", CommonFunctions.GetId()),
                new Claim("userid", token.UserId),
                new Claim("yapilyuserid", token.YapilyUserId),
                new Claim("entityid", token.EntityId),
                new Claim("airslipusertype", token.AirslipUserType.ToString()),
                new Claim("ip", _remoteIpAddressService.GetRequestIP() ?? "UNKNOWN"),
                new Claim("ua", _userAgentService.GetRequestUserAgent() ?? "UNKNOWN")
            };

            return GenerateNewToken(claims);
        }

        public override UserToken GetCurrentToken()
        {
            ClaimsPrincipal claimsPrincipal = _httpContext.User;
            List<Claim> claims = claimsPrincipal.Claims.ToList(); 

            return GenerateTokenFromClaims(claims, claimsPrincipal.Identity?.IsAuthenticated);
        }

        protected override UserToken GenerateTokenFromClaims(ICollection<Claim> tokenClaims, bool? isAuthenticated)
        {
            string correlationId = tokenClaims.GetValue("correlation");
            correlationId = string.IsNullOrWhiteSpace(correlationId) ? Guid.NewGuid().ToString() : correlationId;
            Log.Logger.ForContext(nameof(correlationId), correlationId);

            if (!Enum.TryParse(tokenClaims.GetValue("airslipusertype"), out AirslipUserType airslipUserType))
            {
                airslipUserType = AirslipUserType.Merchant;
            }
            
            return new UserToken(
                isAuthenticated,
                tokenClaims.GetValue("userid"),
                tokenClaims.GetValue("yapilyuserid"),
                tokenClaims.GetValue("entityid"),
                airslipUserType,
                correlationId,
                tokenClaims.GetValue("ip"),
                tokenClaims.GetValue("ua"),
                _httpContext.Request.Headers["Authorization"]
            );
        }
    }
}