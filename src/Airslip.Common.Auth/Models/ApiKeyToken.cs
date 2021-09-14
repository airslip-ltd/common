using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record ApiKeyToken(
        bool? IsAuthenticated,
        string ApiKey,
        string EntityId,
        AirslipUserType AirslipUserType,
        string CorrelationId,
        string IpAddress,
        string BearerToken,
        string Environment
    ) : TokenBase(nameof(ApiKeyToken), IsAuthenticated, CorrelationId, IpAddress, BearerToken);
}