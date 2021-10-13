using System.Collections.Generic;

namespace Airslip.Common.Types.Extensions
{
    public static class DictionaryExtensions
    {
        public static string? GetValue(this IDictionary<string, string> dictionary, string value)
        {
            dictionary.TryGetValue(value, out string? s);

            return s;
        }
        
        public static object? GetValue(this IDictionary<string, object> dictionary, string value)
        {
            dictionary.TryGetValue(value, out object? s);

            return s;
        }
    }
}