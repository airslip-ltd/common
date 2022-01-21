namespace Airslip.Common.Types.Transaction;

public class TransactionRefundItem 
{
    public string? Product_id { get; set; }
    public string? Variant_id { get; set; }
    public string? Order_product_id { get; set; }
    public double? Qty { get; set; }
    public long? Refund { get; set; }
}