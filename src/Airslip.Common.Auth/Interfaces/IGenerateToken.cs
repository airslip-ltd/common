using Airslip.Common.Auth.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface IGenerateToken
    {
        string EntityId { get; init; }
        AirslipUserType AirslipUserType { get; init; }
        List<Claim> GetCustomClaims();
    }
}