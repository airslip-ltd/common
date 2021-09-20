using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;

namespace Airslip.Common.Auth.Extensions
{
    public static class ClaimsExtensions
    {
        public static string GetValue(this IEnumerable<Claim> claims, string type)
        {
            Claim? claim = claims.FirstOrDefault(c => c.Type == type);
            return claim != null ? claim.Value : string.Empty;
        }
    }

    public static class StringExtensions
    {
        public static string GetEnvironment(this IServiceCollection services)
        {
            return services
                .BuildServiceProvider()
                .GetService<IOptions<EnvironmentSettings>>()!
                .Value
                .EnvironmentName;
        }
        
        public static bool IsNullOrWhitespace(this string? s)
        {
            return s == null || string.IsNullOrWhiteSpace(s);
        }

        public static List<string> SplitCsv(this string csvList)
        {
            if (string.IsNullOrWhiteSpace(csvList))
                return new List<string>();

            return csvList
                .TrimEnd(',')
                .Split(',')
                .AsEnumerable<string>()
                .Select(s => s.Trim())
                .ToList();
        }
    }
}