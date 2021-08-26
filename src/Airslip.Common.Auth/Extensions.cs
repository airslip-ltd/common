using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace Airslip.Common.Auth
{
    public static class Extensions
    {
        /// <summary>
        /// Add standard JWT authentication used across the Airslip API estate. Assumes
        /// application settings are available in the format of:
        /// 
        /// appSettings.json:
        /// {
        ///     "JwtSettings": {
        ///         "Key": "Example key",
        ///         "Issuer": "Example issuer",
        ///         "Audience": "Example audience",
        ///         "ExpiresTime": "Example expiry time",
        ///         "ValidateLifetime": "true"
        ///      }
        /// }
        ///
        /// Environment Variables:
        /// JwtSettings:Key = Example key
        /// JwtSettings:Issuer = Example issuer
        /// JwtSettings:Audience = Example audience
        /// JwtSettings:ExpiresTime = Example expiry time
        /// JwtSettings:ValidateLifetime = true
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="configuration">The primary configuration where relevant elements can be found</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddAirslipJwtAuth(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtBearerOptions>()
                .AddScoped<Token>()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer();

            return services;
        }
    }
}