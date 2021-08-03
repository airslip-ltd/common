using System;
using System.Globalization;
using System.Linq;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Types
{
    public static class Culture
    {
        /// <summary>
        ///     Method used to return a currency symbol.
        ///     It receive as a parameter a currency code (3 digits).
        /// </summary>
        /// <param name="currencyCode">3 digits code. Samples GBP, BRL, USD, etc.</param>
        public static string GetCurrencySymbol(string currencyCode)
        {
            RegionInfo regionInfo = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => culture.Name.Length > 0 && !culture.IsNeutralCulture)
                .Select(culture => new { culture, region = new RegionInfo(culture.Name) })
                .Where(t =>
                    string.Equals(t.region.ISOCurrencySymbol, currencyCode, StringComparison.InvariantCultureIgnoreCase))
                .Select(t => t.region).First();

            return regionInfo.CurrencySymbol;
        }

        /// <summary>
        /// Only to be used to get culture info for parsing datetime
        /// </summary>
        /// <param name="currencyCode"></param>
        /// <returns cref="CultureInfo"></returns>
        /// <exception cref="FormatException"></exception>
        public static CultureInfo GetCultureInfoFromCurrencyCode(string currencyCode)
        {
            if (!Enum.TryParse(currencyCode, out Iso4217CurrencyCodes iso4217CurrencyCodes))
                throw new FormatException($"Unable to parse {currencyCode} as enum {nameof(Iso4217CurrencyCodes)}");

            return iso4217CurrencyCodes switch
            {
                Iso4217CurrencyCodes.GBP => new CultureInfo("en-GB"),
                Iso4217CurrencyCodes.USD => new CultureInfo("en-US"),
                // Spanish is used because its the most popular language in EU
                Iso4217CurrencyCodes.EUR => new CultureInfo("es-ES"),
                _ => throw new FormatException($"The currency code {currencyCode} is unsupported")
            };
        }
    }
}