namespace Airslip.Common.Auth.Models
{
    public record GenerateUserToken(
        string UserId,
        string YapilyUserId,
        string Identity
    ) : GenerateTokenBase(nameof(UserToken));
}