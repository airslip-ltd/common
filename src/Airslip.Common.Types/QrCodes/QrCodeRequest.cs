using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Types.QrCodes
{
    public class QrCodeRequest
    {
        public string? TrackingId { get; set; }
        
        public string EntityId { get; set; } = string.Empty;
        
        public AirslipUserType AirslipUserType { get; set; } = AirslipUserType.Merchant;
        
        public string StoreId { get; set; } = string.Empty;
        
        public string CheckoutId { get; set; } = string.Empty;

        public string CallbackUrl { get; set; } = string.Empty;
        
        public List<MatchMetadata> Metadata { get; set; } = new();
    }
}
