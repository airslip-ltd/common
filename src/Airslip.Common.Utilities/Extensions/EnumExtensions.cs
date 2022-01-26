using System;
using System.Linq;

namespace Airslip.Common.Utilities.Extensions
{
    public static class EnumExtensions
    {
        public static string GetCsv<TEnum>(bool excludeFirst = false) where TEnum : struct, Enum
        {
            string[] names = Enum.GetNames<TEnum>();
            return excludeFirst 
                ? names.Skip(1).ToArray().ToCsv() 
                : names.ToCsv();
        }
        
        public static bool TryParseIgnoreCase<TEnum>(string value, out TEnum parsedObject) where TEnum : struct
        {
            return Enum.TryParse<TEnum>(value, true, out parsedObject);
        }
    }
}