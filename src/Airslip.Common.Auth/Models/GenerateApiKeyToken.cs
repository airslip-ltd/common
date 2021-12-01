using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class GenerateApiKeyToken : IGenerateToken
    {
        public GenerateApiKeyToken(string entityId, string apiKey, AirslipUserType airslipUserType)
        {
            EntityId = entityId;
            ApiKey = apiKey;
            AirslipUserType = airslipUserType;
        }

        public string EntityId { get; init; }
        public string ApiKey { get; init; }
        public AirslipUserType AirslipUserType { get; init; }
        public List<Claim> GetCustomClaims(TokenEncryptionSettings settings)
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.API_KEY, ApiKey.Encrypt(settings))
            };

            return claims;
        }
    }
}