using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Validator;

namespace Airslip.Common.Matching.Response
{
    public record MatchResponse : ISuccess
    {
        public string? TrackingId { get; init; }
        public string MatchType { get; set; } = string.Empty;
        public bool Success { get; init; }
        public ValidationMessageModel? Error { get; set; }
    }
}
