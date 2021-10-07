using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Matching.Enum;

namespace Airslip.Common.Types.Matching.Response
{
    public class QuestionResponse : ISuccess
    {
        public QuestionResponseStatus Status { get; init; }
        public string? ErrorMessage { get; init; }
        public string? QuestionId { get; set; }
        public QuestionDetailResponse? Question { get; set; }
    }
}