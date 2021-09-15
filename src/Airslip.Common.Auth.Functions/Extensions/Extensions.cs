using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Functions.Implementations;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Auth.Schemes;
using Microsoft.Extensions.DependencyInjection;
using Airslip.Common.Types.Configuration;
using Microsoft.Extensions.Configuration;

namespace Airslip.Common.Auth.Functions.Extensions
{
    public static class Extensions
    {
        /// <summary>
        /// Add ApiKey Validation for use in Function Apps
        /// </summary>
        /// <param name="services">The service collection to append services to</param>
        /// <param name="withEnvironment">The name of the environment which will be used for validating API Keys</param>
        /// <returns>The updated service collection</returns>
        public static IServiceCollection AddAirslipFunctionAuth(this IServiceCollection services, 
            IConfiguration configuration, string? withEnvironment = null)
        {
            services
                .AddScoped<IApiKeyRequestDataHandler, ApiKeyRequestDataHandler>()
                .AddScoped<IClaimsPrincipalLocator, HttpContextPrincipalLocator>()
                .AddScoped<IHttpHeaderLocator, HttpContextHeaderLocator>()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .AddScoped<ITokenDecodeService<ApiKeyToken>, TokenDecodeService<ApiKeyToken>>()
                .AddScoped<ITokenValidator<ApiKeyToken>, TokenValidator<ApiKeyToken>>();
                
            services
                .AddAuthentication(ApiKeyAuthenticationSchemeOptions.ApiKeyScheme)
                .AddApiKeyAuth(opt =>
                {
                    opt.Environment = withEnvironment ?? services.GetEnvironment();
                });

            return services;
        }
    }
}