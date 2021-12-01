using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class UserToken : TokenBase
    {
        public string UserId { get; private set; } = "";
        public string YapilyUserId { get; private set; } = "";

        public override void SetCustomClaims(List<Claim> tokenClaims, TokenEncryptionSettings settings)
        {
            UserId = tokenClaims.GetValue(AirslipClaimTypes.USER_ID).Decrypt(settings);
            YapilyUserId = tokenClaims.GetValue(AirslipClaimTypes.YAPILY_USER_ID).Decrypt(settings);
        }
    }
}