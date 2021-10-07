using Airslip.Common.Types.Matching.Enum;
using System.Collections.Generic;

namespace Airslip.Common.Types.Matching.Response
{
    public class QuestionDetailResponse
    {
        public TransactionQuestionType QuestionType { get; set; }
        
        public string? QuestionText { get; init; }
        
        public List<string>? Options { get; init; }
    }
}