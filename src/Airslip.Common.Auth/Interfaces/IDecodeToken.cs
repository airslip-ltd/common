using Airslip.Common.Auth.Enums;
using System.Collections.Generic;
using System.Security.Claims;

namespace Airslip.Common.Auth.Interfaces
{
    public interface IDecodeToken
    {
        string TokenType { get; init; }
        bool? IsAuthenticated { get; init; }
        string CorrelationId { get; init; }
        string IpAddress { get; init; }
        string BearerToken { get; init; }
        string UserAgent { get; init; }
        string EntityId { get; init; }
        AirslipUserType AirslipUserType { get; init; }
        string Environment { get; init; }
        
        void SetCustomClaims(List<Claim> tokenClaims);
    }
}