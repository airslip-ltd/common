namespace Airslip.Common.Types.Transaction;

public class TransactionRefundItem 
{
    public string? TransactionProductId { get; init; }
    public string? ProductId { get; init; }
    public string? VariantId { get; init; }
    public double? Qty { get; set; }
    public long? Refund { get; set; }
}