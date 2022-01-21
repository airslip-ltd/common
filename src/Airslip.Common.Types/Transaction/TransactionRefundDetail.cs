using System.Collections.Generic;

namespace Airslip.Common.Types.Transaction;

public class TransactionRefundDetail 
{
    public string? Id { get; set; }
    public long? Shipping { get; set; }
    public long? Fee { get; set; }
    public long? Tax { get; set; }
    public long? Total { get; set; }
    public IntegrationDateTime? ModifiedTime { get; set; }
    public string? Comment { get; set; }
    public ICollection<TransactionRefundItem>? Items { get; set; }
}