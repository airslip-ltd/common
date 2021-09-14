using Airslip.Common.Auth.Enums;

namespace Airslip.Common.Auth.Models
{
    public record QrCodeToken(
        string StoreId,
        string CheckoutId,
        string EntityId,
        AirslipUserType AirslipUserType,
        string CorrelationId,
        string QrCodeKey, 
        string Environment
    ) : TokenBase(nameof(QrCodeToken), false, CorrelationId, "", "");
}