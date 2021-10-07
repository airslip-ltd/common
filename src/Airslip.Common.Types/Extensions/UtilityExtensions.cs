using Airslip.Common.Types.Configuration;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Types.Extensions
{
    public static class UtilityExtensions
    {
        public static bool InList<TType>(this TType value, params TType[] values)
        {
            return ((IList) values).Contains(value);
        }

        public static PublicApiSetting? GetSettingByName(this PublicApiSettings settings, string name)
        {
            if (settings.Settings?.FirstOrDefault(o => o.Key.Equals(name)) == null) 
                return null;

            return settings
                .Settings
                .First(o => o.Key.Equals(name))
                .Value;
        }
        
        public static string ToBaseUri(this PublicApiSetting setting)
        {
            List<string> parts = new()
            {
                setting.BaseUri.RemoveLeadingAndTrailing("/"),
                setting.UriSuffix.RemoveLeadingAndTrailing("/")
            };
            parts.RemoveAll(string.IsNullOrWhiteSpace);
            return string.Join("/", parts);
        }

        private static string RemoveLeadingAndTrailing(this string fromValue, string removeValue)
        {
            if (fromValue.EndsWith(removeValue)) fromValue = fromValue.Remove(fromValue.Length - 1, 1);
            if (fromValue.StartsWith(removeValue)) fromValue = fromValue[1..];
            return fromValue;
        }
    }
}