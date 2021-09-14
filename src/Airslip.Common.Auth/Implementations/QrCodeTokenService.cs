using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
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
    public class QrCodeTokenService : TokenService<QrCodeToken, GenerateQrCodeToken>
    {
        private readonly HttpContext _httpContext;

        public QrCodeTokenService(IHttpContextAccessor httpContextAccessor, 
            IOptions<JwtSettings> jwtSettings)
        : base(jwtSettings)
        {
            _httpContext = httpContextAccessor.HttpContext!;
        }

        public override NewToken GenerateNewToken(GenerateQrCodeToken token)
        {
            List<Claim> claims = new()
            {
                new Claim("correlation", CommonFunctions.GetId()),
                new Claim("storeid", token.StoreId),
                new Claim("checkoutid", token.CheckoutId),
                new Claim("qrcodekey", token.QrCodeKey),
                new Claim("environment", QrCodeAuthenticationSchemeOptions.ThisEnvironment),
                new Claim("airslipusertype", token.AirslipUserType.ToString()),
                new Claim("entityid", token.EntityId)
            };

            return GenerateNewToken(claims);
        }
        
        public override QrCodeToken GetCurrentToken()
        {
            ClaimsPrincipal claimsPrincipal = _httpContext.User;

            return GenerateTokenFromClaims(claimsPrincipal.Claims.ToList(), claimsPrincipal.Identity?.IsAuthenticated);
        }

        protected override QrCodeToken GenerateTokenFromClaims(ICollection<Claim> tokenClaims, bool? isAuthenticated)
        {
            string correlationId = tokenClaims.GetValue("correlation");
            correlationId = string.IsNullOrWhiteSpace(correlationId) ? CommonFunctions.GetId() : correlationId;
            Log.Logger.ForContext(nameof(correlationId), correlationId);

            if (!Enum.TryParse(tokenClaims.GetValue("airslipusertype"), out AirslipUserType airslipUserType))
            {
                airslipUserType = AirslipUserType.Merchant;
            }
            
            return new QrCodeToken(
                tokenClaims.GetValue("storeid"),
                tokenClaims.GetValue("checkoutid"),
                tokenClaims.GetValue("entityid"),
                airslipUserType,
                correlationId,
                tokenClaims.GetValue("qrcodekey"),
                tokenClaims.GetValue("environment")
            );
        }
    }
}