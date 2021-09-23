namespace Airslip.Common.Types.Transaction
{
    public record CardDetailRequest
    {
        /// <summary>
        /// The authorization code that confirms the debit or credit card transaction is approved.
        /// </summary>
        /// <example>212908</example>
        //[Required]
        //[MaxLength(7)]
        public string? AuthCode { get; init; }

        /// <summary>
        /// An application unique identifier (AID) is used to address an application in the card or Host Card Emulation (HCE) if delivered without a card.
        /// An AID consists of a registered application provider identifier (RID) of five bytes, which is issued by the ISO/IEC 7816-5 registration authority.
        /// </summary>
        /// <example>A0000000031010</example>
        public string? Aid { get; init; }

        /// <summary>
        /// The unique identifier called a terminal ID that is tied to each terminal.
        /// </summary>
        /// <example>00000251</example>
        public string? Tid { get; init; }

        /// <summary>
        /// The masked Primary Account Number (PAN) used to facilitate the transaction. Minimum of four characters.
        /// </summary>
        /// <example>************0109</example>
        //[Required]
        public string? MaskedPanNumber { get; init; }

        /// <summary>
        /// A Primary Account Number (PAN) Sequence Number identifies and differentiates cards with the same PAN.
        /// </summary>
        /// <example>00</example>
        public string? PanSequence { get; init; }

        /// <summary>
        /// The central payment network of the card, i.e Visa, Mastercard, American Express.
        /// </summary>
        /// <example>visa</example>
        //[Required]
        public string? CardScheme { get; init; }
    }
}