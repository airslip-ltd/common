using Airslip.Common.Types.Validator;

namespace Airslip.Common.Types.Matching
{
    public record MatchResponse
    {
        public string? TrackingId { get; init; }
        public string MatchType { get; set; } = string.Empty;
        public bool Success { get; init; }
        public ValidationMessageModel? Error { get; set; }
    }
}
