using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record GenerateApiKeyToken(
        string ApiKey,
        string EntityId,
        AirslipUserType AirslipUserType
    ) : GenerateTokenBase(nameof(ApiKeyToken));
}