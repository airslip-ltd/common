namespace Airslip.Common.Types.Transaction
{
    public record TransactionDetailRequest
    {
        /// <summary>
        /// The date at which the transaction was created, in standard ISO 8601 format.
        /// </summary>
        /// <example>2021-08-26T14:26:29+01:00</example>
        public string? Date { get; init; }
        
        /// <summary>
        /// The time of this transaction, in hh:mm:ss format.
        /// </summary>
        /// <example>14:26:29</example>
        public string? Time { get; init; }
        
        /// <summary>
        /// The number of the till in the store.
        /// </summary>
        /// <example>002</example>
        public string? Till { get; init; }
        
        /// <summary>
        /// The transaction number. Usually the count of the day's transactions.
        /// </summary>
        /// <example>000184</example>
        public string? Number { get; init; }
        
        /// <summary>
        /// The unique reference of the store.
        /// </summary>
        /// <example>001</example>
        public string? Store { get; init; }
    }
}