using Airslip.Common.Matching.Data;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Transaction;
using System.Collections.Generic;

namespace Airslip.Common.Matching.Response
{
    public record MatchResultResponse(MatchTypes MatchType,
        string TransactionTrackingId,
        string MatchTrackingId,
        MatchLikelihood MatchLikelihood) : ISuccess
    {
        public string? UserId { get; set; } = string.Empty;
        public string? EntityId { get; set; } = string.Empty;
        public AirslipUserType AirslipUserType { get; set; } = AirslipUserType.Standard;
        public List<MatchMetadata> Metadata { get; } = new();
    } 
}