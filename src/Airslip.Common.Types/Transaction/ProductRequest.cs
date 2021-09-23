using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Airslip.Common.Types.Transaction
{
    public record ProductRequest
    {
        /// <summary>
        /// The name of the product.
        /// </summary>
        /// <example>Coffee Machine</example>
        //[Required]
        public string? Item { get; init; }
        
        /// <summary>
        /// The code of the product.
        /// </summary>
        /// <example>PR1234</example>
        //[Required]
        public string? Code { get; init; }

        /// <summary>
        /// Total of all items before discounts or taxes are applied.
        /// </summary>
        /// <example>18530</example>
        //[Required]
        public long? Subtotal { get; init; }
        
        /// <summary>
        /// Total of all items after discounts and taxes are applied.
        /// </summary>
        /// <example>20846</example>
        //[Required]
        public long? Total { get; init; }

        /// <summary>
        /// The total number of products purchased for a particular item.
        /// </summary>
        /// <example>1</example>
        //[Required]
        public int? Quantity { get; init; }

        /// <summary>
        /// The longer description for the item.
        /// </summary>
        /// <example>The Barista Express is here to give you the ultimate cup of coffee. Designed on the principle that ingredients taste better when fresh.</example>
        //[Required]
        public string? Description { get; init; }

        /// <summary>
        /// The warranty end date if the product has a warranty. Measured in seconds since the Unix epoch.
        /// </summary>
        /// <example>1623406801556.</example>
        public long? WarrantyExpiryDateTime { get; init; }

        /// <summary>
        /// The product image URL.
        /// </summary>
        /// <example>https://example.com/coffee-machine-url.png</example>
        [DataType(DataType.ImageUrl)]
        public string? ImageUrl { get; init; }

        /// <summary>
        /// A URL of a publicly-accessible webpage for this product. This is used when a user shares a product so would be valuable if the product can be purchased directly from this URL.
        /// </summary>
        /// <example>https://example.com/buy-a-coffee-machine</example>
        [DataType(DataType.Url)]
        public string? Url { get; init; }

        /// <summary>
        /// A URL to the manual or instructions if the item needs to be assembled. Usually in PDF format.
        /// </summary>
        /// <example>https://www.example.com/gb/en/assembly_instructions/barista-express-by-sage.pdf</example>
        public string? ManualUrl { get; init; }

        /// <summary>
        /// The URLs to any videos for help and guidance or any other relevant purpose.
        /// </summary>
        /// <example>31.00 x 40.00 x 33.00 cm</example>
        public IEnumerable<VideoTutorialRequest>? VideoTutorials { get; init; }

        /// <summary>
        /// The height, width and depth for this product.
        /// </summary>
        /// <example>31.00 x 40.00 x 33.00 cm</example>
        public string? Dimensions { get; init; }

        /// <summary>
        /// The date of release, past or future. Measured in seconds since the Unix epoch.
        /// </summary>
        /// <example>1623401201556</example>
        public long? ReleaseDate { get; init; }

        /// <summary>
        /// The manufacturer of this product.
        /// </summary>
        /// <example>Example</example>
        public string? Manufacturer { get; init; }

        /// <summary>
        /// The unique identifier given to each product.
        /// </summary>
        /// <example>B077YZXR1W</example>
        public string? ModelNumber { get; init; }

        /// <summary>
        /// The internal identification number assigned to each product and their variants.
        /// </summary>
        /// <example>KS93528TUT</example>
        public string? Sku { get; init; }

        /// <summary>
        /// The European Article Number to aid in identifying a particular item. Now popular across the world and also called International Article Number.
        /// </summary>
        /// <example>4070071967072</example>
        public string? Ean { get; init; }

        /// <summary>
        /// The Universal Product Code to aid in identifying a particular item. Usually used in USA and Canada.
        /// </summary>
        /// <example>036000291452</example>
        public string? Upc { get; init; }

        /// <summary>
        /// The VAT rate of the product along with its description. Different category of products have different VAT rates in some countries.
        /// </summary>
        /// <example>"SomeMoreData", "AnotherValue"</example>
        public VatRequest? VatRate { get; init; }
        
        /// <summary>
        /// Any additional key value pairs should be constructed as metadata here.
        /// </summary>
        public Dictionary<string, object>? Metadata { get; init; }
    }
}
