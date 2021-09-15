using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Implementations;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class UserToken : TokenBase
    {
        public string UserId { get; private set; } = "";
        public string YapilyUserId { get; private set; } = "";

        public override void SetCustomClaims(List<Claim> tokenClaims)
        {
            UserId = tokenClaims.GetValue(AirslipClaimTypes.USER_ID);
            YapilyUserId = tokenClaims.GetValue(AirslipClaimTypes.YAPILY_USER_ID);
        }
    }
}