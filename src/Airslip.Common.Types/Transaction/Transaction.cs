using System.Collections.Generic;

namespace Airslip.Common.Types.Transaction
{
    public record Transaction
    {
        /// <summary>
        /// The internally generated ID.
        /// </summary>
        /// <example>500d96aa1c2f4f26b761db6bd7b4a5d5</example>
        public string? InternalId { get; init; }
        
        /// <summary>
        /// The name of the POS or e-commerce provider. 
        /// </summary>
        /// <example>SWAN_RETAIL</example>
        public string? Source { get; init; }

        /// <summary>
        /// The unique ID of the transaction such as order number. Like all Airslip identifiers, the transactionNumber is case sensitive.
        /// </summary>
        /// <example>372931583dd44034a6118002c252bc38</example>
        //[Required]
        public string? TransactionNumber { get; init; }

        /// <summary>
        /// The date at which the transaction was created, in standard ISO 8601 format.
        /// </summary>
        /// <example>2021-08-26T14:26:29+01:00</example>
        //[Required]
        public string? Datetime { get; init; }
        
        /// <summary>
        /// The exact text that will appear on the transactions bank statement.
        /// </summary>
        /// <example>CARD PAYMENT TO EXAMPLE LIMITED,35.00 GBP, RATE 1.00/GBP ON 31-12-2021* 12345</example>
        //[Required]
        public string? BankStatementDescription { get; init; }

        /// <summary>
        /// The five digit dynamic suffix identifier appended to the bank statement description. One identifier used to help match with the bank transaction. 
        /// </summary>
        /// <example>12345</example>
        //[Required]
        public string? BankStatementTransactionIdentifier { get; init; }

        /// <summary>
        /// The unique identifier set to identify a merchants store.
        /// </summary>
        /// <example>A356</example>
        //[Required]
        public string? StoreLocationId { get; init; }
        
        /// <summary>
        /// If no store location id can be provided then the start of a store location address can.
        /// </summary>
        /// <example>42 Harbour Road</example>
        //[Required]
        public string? StoreAddress { get; init; }

        /// <summary>
        /// A list of products for this transaction.
        /// </summary>
        public ICollection<ProductRequest>? Products { get; init; }

        /// <summary>
        /// Was this transaction made in-store or online?
        /// </summary>
        /// <example>false</example>
        //[Required]
        //[MaxLength(7)]
        public bool? OnlinePurchase { get; init; }

        /// <summary>
        /// The total of the transaction before any taxes, discounts or any other additional charges are added.
        /// </summary>
        /// <example>18530</example>
        //[Required]
        public long? Subtotal { get; init; }

        /// <summary>
        /// A fee collected to pay for services related to the primary product or service being purchased.
        /// </summary>
        /// <example>2316</example>
        public long? ServiceCharge { get; init; }

        /// <summary>
        /// The total reduction on the original priced transaction. 
        /// </summary>
        public IEnumerable<DiscountRequest>? Discounts { get; init; }

        /// <summary>
        /// The VAT amount of the transaction along with its rate. Different category of products have different VAT rates in some countries.
        /// </summary>
        public IEnumerable<VatRequest>? VatRates { get; init; }

        /// <summary>
        /// The total amount including taxes, service charges and any other additional charges.
        /// </summary>
        /// <example>20846</example>
        //[Required]
        public long? Total { get; init; }

        /// <summary>
        /// The three digit currency code in ISO 4217 format.
        /// </summary>
        /// <example>GBP</example>
        //[Required]
        public string? CurrencyCode { get; init; }
        
        /// <summary>
        /// The email of the customer if applicable.
        /// </summary>
        /// <example>noah.dejong@examples.com</example>
        public string? CustomerEmail { get; init; }

        /// <summary>
        /// The name of the cashier who has served the customer for this transaction.
        /// </summary>
        /// <example>Noah De Jong</example>
        public string? OperatorName { get; init; }
        
        /// <summary>
        /// Further properties about the transaction. Details are used to generated a barcode.
        /// </summary>
        public TransactionDetailRequest? TransactionDetail { get; init; }
        
        /// <summary>
        /// A collection for the methods of payments.
        /// </summary>
        public IEnumerable<PaymentDetailRequest>? PaymentDetails { get; init; }
        
        /// <summary>
        /// Any additional key value pairs should be constructed as metadata here.
        /// </summary>
        public Dictionary<string, object>? Metadata { get; init; }
    }
}
