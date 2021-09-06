using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record ApiKeyToken(
        bool? IsAuthenticated,
        string ApiKey,
        string EntityId,
        ApiKeyUsageType ApiKeyUsageType,
        string CorrelationId,
        string IpAddress,
        string BearerToken
    ) : TokenBase(nameof(ApiKeyToken), IsAuthenticated, CorrelationId, IpAddress, BearerToken);
}