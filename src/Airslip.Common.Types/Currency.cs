using System;
using System.Globalization;
using System.Linq;

namespace Airslip.Common.Types
{
    public static class Currency
    {
        /// <summary>
        ///     Method used to return a currency symbol.
        ///     It receive as a parameter a currency code (3 digits).
        /// </summary>
        /// <param name="code">3 digits code. Samples GBP, BRL, USD, etc.</param>
        public static string GetSymbol(string code)
        {
            RegionInfo regionInfo = CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(culture => culture.Name.Length > 0 && !culture.IsNeutralCulture)
                .Select(culture => new { culture, region = new RegionInfo(culture.Name) })
                .Where(t =>
                    string.Equals(t.region.ISOCurrencySymbol, code, StringComparison.InvariantCultureIgnoreCase))
                .Select(t => t.region).First();

            return regionInfo.CurrencySymbol;
        }

        public static long? ConvertToUnit(string? source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            bool canParse = double.TryParse(source, out double value);

            return canParse ? ConvertToUnit(value) : (long)value;
        }
        
        public static long? ConvertToUnit(decimal? value)
        {
            if (value is null)
                return null;

            return Convert.ToInt64(value * 100);
        }
        
        public static long? ConvertToUnit(double? value)
        {
            if (value is null)
                return null;

            return Convert.ToInt64(value * 100);
        }
    }
}