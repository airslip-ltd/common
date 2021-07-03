using System;
using System.Collections.Generic;

namespace Airslip.Common.Types
{
    public static class Alpha2CountryCodes
    {
        private static readonly Dictionary<string, string> _mapper = new()
        {
            { "FR", "France" }, { "GB", "United Kingdom" }, { "IE", "Ireland" }, { "NL", "Netherlands" }
        };

        public static string Parse(string value)
        {
            bool canParse = _mapper.TryGetValue(value, out string? countryDescription);

            if (!canParse)
                throw new Exception($"Unable to parse {value}");

            return countryDescription!;
        }
        
        
        public static bool TryParse(string code, out string? value)
        {
            bool canParse = _mapper.TryGetValue(code, out string? countryDescription);

            if (!canParse)
                throw new Exception($"Unable to parse {code}");

            value = countryDescription;
            
            return canParse;
        }
    }
}