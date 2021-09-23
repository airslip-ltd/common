namespace Airslip.Common.Types.Transaction
{
    public record VatRequest
    {
        /// <summary>
        /// The tax code 
        /// </summary>
        /// <example>S</example>
        public string? Code { get; init; }
        
        /// <summary>
        /// The tax rate from 0-100. 
        /// </summary>
        /// <example>20.0</example>
        public decimal? Rate { get; init; }
        
        /// <summary>
        /// The taxable amount.
        /// </summary>
        /// <example>333</example>
        //[Required]
        public long? Amount { get; init; }
    }
}