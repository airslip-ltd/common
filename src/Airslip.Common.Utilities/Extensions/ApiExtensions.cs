using Microsoft.Extensions.Configuration;

namespace Airslip.Common.Utilities.Extensions
{
    public class ApiExtensions
    {
        public static T GetConfigurationSection<T>(IConfiguration configuration)
        {
            string className = typeof(T).Name;
            IConfigurationSection configurationSection = configuration.GetSection(className);
            return configurationSection.Get<T>();
        }
    }
}