using System.Collections.Generic;

namespace Airslip.Common.Types.Transaction;

public record TransactionPaymentDetail
{
    /// <summary>
    /// The method of payment. Can be either card, cash, giftcard, voucher or coupon.
    /// </summary>
    /// <example>card</example>
    public string? Method { get; init; }
        
    /// <summary>
    /// The amount of the other method of payment.
    /// </summary>
    /// <example>20846</example>
    public long? Amount { get; init; }
        
    /// <summary>
    /// Further details about the card.
    /// </summary>
    //[Required]
    public IEnumerable<TransactionCardDetail>? CardDetails { get; init; }
}