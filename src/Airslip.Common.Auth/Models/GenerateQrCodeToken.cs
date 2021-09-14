using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record GenerateQrCodeToken(
        string StoreId,
        string CheckoutId,
        string EntityId,
        AirslipUserType AirslipUserType,
        string QrCodeKey
    ) : GenerateTokenBase(nameof(ApiKeyToken));
}