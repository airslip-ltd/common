using Airslip.Common.Types.Configuration;
using System.Collections;
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
    }
}