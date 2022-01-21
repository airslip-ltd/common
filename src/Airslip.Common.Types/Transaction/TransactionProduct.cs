using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airslip.Common.Types.Transaction;

[UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
public class TransactionProduct 
{
    [Obsolete] public string? Item => Name;
    [Obsolete] public string? Code => ProductCode;
    [Obsolete] public long? Subtotal => Price;
    [Obsolete] public long? Total => TotalPrice;
    [Obsolete] public TransactionVat? VatRate { get; init; }
    
    public string? TransactionProductId { get; init; }
    public string? ParentTransactionProductId { get; init; }
    public string? ProductId { get; init; }
    public string? Manufacturer { get; init; }
    public string? ModelNumber { get; init; }
    public string? ProductCode { get; init; }
    public string? Name { get; init; }
    public string? Description { get; init; }
    public long? Price { get; init; }
    public long? PriceIncTax { get; init; }
    public double? Quantity { get; init; }
    public long? DiscountAmount { get; init; }
    public long? TotalPrice { get; init; }
    public double? TaxPercent { get; init; }
    public long? TaxValue { get; init; }
    public long? TaxValueAfterDiscount { get; init; }
    public string? VariantId { get; init; }
    public string? WeightUnit { get; init; }
    public double? Weight { get; init; }
    public string? Sku { get; init; }
    public string? Ean { get; init; }
    public string? Upc { get; init; }

    public long? WarrantyExpiryDateTime { get; init; }
    [DataType(DataType.ImageUrl)]
    public string? ImageUrl { get; init; }
    [DataType(DataType.Url)]
    public string? Url { get; init; }
    public string? ManualUrl { get; init; }
    public IEnumerable<VideoTutorialRequest>? VideoTutorials { get; init; }
    public string? Dimensions { get; init; }
}