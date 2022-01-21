using System.Collections.Generic;

namespace Airslip.Common.Types.Transaction;

public class TransactionStatus
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public ICollection<TransactionHistory>? History { get; set; }
    public TransactionRefundSummary? Refund_info { get; set; }
}