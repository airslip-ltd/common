using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record GenerateUserToken(
        string UserId,
        string YapilyUserId,
        string EntityId,
        AirslipUserType AirslipUserType
    ) : GenerateTokenBase(nameof(UserToken));
}