namespace Airslip.Common.Auth.Models
{
    public record UserToken(
        bool? IsAuthenticated,
        string UserId,
        string YapilyUserId,
        string Identity,
        string CorrelationId,
        string IpAddress,
        string UserAgent,
        string BearerToken
    ) : TokenBase(nameof(UserToken), IsAuthenticated, CorrelationId, IpAddress, BearerToken);
}