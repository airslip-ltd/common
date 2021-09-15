using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Enums;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
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

        public List<Claim> GetCustomClaims()
        {
            List<Claim> claims = new()
            {
                new Claim(AirslipClaimTypes.USER_ID, UserId),
                new Claim(AirslipClaimTypes.YAPILY_USER_ID, YapilyUserId)
            };

            return claims;
        }
    }
}