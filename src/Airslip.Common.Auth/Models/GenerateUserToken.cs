using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Types.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Models
{
    public class GenerateUserToken : IGenerateToken
    {
        public GenerateUserToken(string entityId, AirslipUserType airslipUserType, string userId, string yapilyUserId)
        {
            EntityId = entityId;
            AirslipUserType = airslipUserType;
            UserId = userId;
            YapilyUserId = yapilyUserId;
        }

        public string EntityId { get; init; }
        public AirslipUserType AirslipUserType { get; init; }
        public string UserId { get; init; }
        public string YapilyUserId { get; init; }

        public List<Claim> GetCustomClaims(TokenEncryptionSettings settings)
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.USER_ID, UserId.Encrypt(settings)),
                new Claim(AirslipClaimTypes.YAPILY_USER_ID, YapilyUserId.Encrypt(settings))
            };

            return claims;
        }
    }
}