using System.Collections.Generic;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Transaction;
using Airslip.Common.Types.Validator;

namespace Airslip.Matching.QrCodes.Core.Models
{
    public record QrCodeResponse
    {
        public string? TrackingId { get; init; }
        public bool Success { get; init; }
        public ValidationMessageModel? Error { get; set; }
    }
}
