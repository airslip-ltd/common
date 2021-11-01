using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Request
{
    public class MatchRequest
    {
        public string? TrackingId { get; set; }

        public string MatchType { get; set; } = string.Empty;
        
        public string EntityId { get; set; } = string.Empty;
        
        public AirslipUserType AirslipUserType { get; set; } = AirslipUserType.Merchant;
        
        public string StoreId { get; set; } = string.Empty;
        
        public string CheckoutId { get; set; } = string.Empty;

        public string CallbackUrl { get; set; } = string.Empty;
        
        public List<MatchMetadata> Metadata { get; set; } = new();
    }
}
