using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record UserToken(
        bool? IsAuthenticated,
        string UserId,
        string YapilyUserId,
        string EntityId,
        AirslipUserType AirslipUserType,
        string CorrelationId,
        string IpAddress,
        string UserAgent,
        string BearerToken
    ) : TokenBase(nameof(UserToken), IsAuthenticated, CorrelationId, IpAddress, BearerToken);
}