using Airslip.Common.Auth.Data;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Functions.Configuration;
using Airslip.Common.Auth.Functions.Implementations;
using Airslip.Common.Auth.Functions.Interfaces;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Airslip.Common.Types.Configuration;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.Configuration;

namespace Airslip.Common.Auth.Functions.Extensions
{
    public static class ServiceExtensions
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
                .Configure<TokenEncryptionSettings>(configuration.GetSection(nameof(TokenEncryptionSettings)))
                .AddScoped<IApiRequestAuthService, ApiRequestAuthService>()
                .AddScoped<IApiKeyRequestDataHandler, ApiKeyRequestDataHandler>()
                .AddScoped<IFunctionContextAccessor, FunctionContextAccessor>()
                .AddScoped<IClaimsPrincipalLocator, FunctionContextPrincipalLocator>()
                .AddScoped<IHttpContentLocator, FunctionContextHeaderLocator>()
                .Configure<JwtSettings>(configuration.GetSection(nameof(JwtSettings)))
                .Configure<EnvironmentSettings>(configuration.GetSection(nameof(EnvironmentSettings)))
                .Configure<ApiAccessSettings>(configuration.GetSection(nameof(ApiAccessSettings)))
                .AddScoped<ITokenDecodeService<ApiKeyToken>, TokenDecodeService<ApiKeyToken>>()
                .AddScoped<ITokenValidator<ApiKeyToken>, TokenValidator<ApiKeyToken>>()
                .AddScoped<IUserContext, ApiKeyTokenUserService>();

            AirslipSchemeOptions.ThisEnvironment = services.GetEnvironment();

            return services;
        }
    }
}