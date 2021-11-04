using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Interfaces;
using System;
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

        public static TReturnType GetSettingByName<TReturnType>(this SettingCollection<TReturnType> settings, string name)
        {
            KeyValuePair<string, TReturnType>? result = settings.Settings?
                .FirstOrDefault(o => o.Key.Equals(name));
            
            if (result == null)
                throw new ArgumentException($"{nameof(TReturnType)}:Settings:{name} " +
                                            $"section missing from appSettings", name);

            return result.Value.Value;
        }

        public static PublicApiSetting GetSettingByName(this PublicApiSettings settings, string name)
        {
            return settings.GetSettingByName<PublicApiSetting>(name);
        }
        
        public static string ToBaseUri(this PublicApiSetting setting)
        {
            List<string> parts = new()
            {
                setting.BaseUri.RemoveLeadingAndTrailing("/"),
                setting.UriSuffix.RemoveLeadingAndTrailing("/"),
                setting.Version.RemoveLeadingAndTrailing("/")
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