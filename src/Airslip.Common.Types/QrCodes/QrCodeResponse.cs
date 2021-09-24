using Airslip.Common.Types.Validator;

namespace Airslip.Common.Types.QrCodes
{
    public record QrCodeResponse
    {
        public string? TrackingId { get; init; }
        public bool Success { get; init; }
        public ValidationMessageModel? Error { get; set; }
    }
}
