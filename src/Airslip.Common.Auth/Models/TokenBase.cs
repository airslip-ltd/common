namespace Airslip.Common.Auth.Models
{
    public abstract record TokenBase
    (
        string TokenType,
        bool? IsAuthenticated,
        string CorrelationId,
        string IpAddress,
        string BearerToken
    );
}