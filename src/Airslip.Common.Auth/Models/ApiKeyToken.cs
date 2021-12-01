using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class ApiKeyToken : TokenBase
    {
        public string ApiKey { get; private set; } = "";
        
        public override void SetCustomClaims(List<Claim> tokenClaims, TokenEncryptionSettings settings)
        {
            ApiKey = tokenClaims.GetValue(AirslipClaimTypes.API_KEY).Decrypt(settings);
        }
    }
}