using System.Collections.Generic;

namespace Airslip.Common.Types.Transaction;

public class TransactionRefundSummary 
{
    public long? Shipping { get; set; }
    public long? Fee { get; set; }
    public long? Tax { get; set; }
    public long? TotalRefunded { get; set; }
    public IntegrationDateTime? Time { get; set; }
    public string? Comment { get; set; }
    public ICollection<TransactionRefundItem>? RefundedItems { get; set; }
}