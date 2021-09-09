using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record GenerateQrCodeToken(
        string StoreId,
        string CheckoutId,
        string EntityId,
        AirslipUserType AirslipUserType
    ) : GenerateTokenBase(nameof(ApiKeyToken));
}