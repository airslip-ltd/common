using Airslip.Common.Services.Consent.Configuration;
using Airslip.Common.Services.Consent.Implementations;
using Airslip.Common.Services.Consent.Interfaces;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Airslip.Common.Services.Consent.Extensions
{
    public static class Extensions
    {
        private static readonly Regex regEx = new Regex(@"\{(\w*?)\}", RegexOptions.Compiled);
        public static string ApplyReplacements(this string format, Dictionary<string, string> replaceWith)
        {
            return regEx.Replace(format, delegate(Match match)
            {
                string key = match.Groups[1].Value;
                return replaceWith[key];
            });
        }
        
        
        /// <summary>
        ///  Adds consent authorisation ca[abilities to your app
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static IServiceCollection AddConsentAuthorisation(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services
                .AddHttpClient()
                .Configure<SettingCollection<ProviderSetting>>(configuration.GetSection("ProviderSettings"))
                .AddScoped<IProviderDiscoveryService, ProviderDiscoveryService>()
                .AddScoped<IProviderConsentService, ProviderConsentService>();

            return services;
        }
        
    }
}