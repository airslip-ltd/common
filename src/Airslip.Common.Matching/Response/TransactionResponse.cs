using Airslip.Common.Matching.Enum;
using Airslip.Common.Types.Interfaces;
using Airslip.Common.Types.Transaction;

namespace Airslip.Common.Matching.Response
{
    public class TransactionResponse : ISuccess
    {
        public TransactionResponseStatus Status { get; init; }
        public string? ErrorMessage { get; init; }
        public TransactionDetails? Transaction { get; set; }
    }
}