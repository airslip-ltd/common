using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.AppIdentifiers
{
    public static class ServiceExtensions
    {
        public static IServiceCollection AddAppIdentifiers(
            this IServiceCollection services, 
            IConfiguration configuration)
        {
            services
                .Configure<AppleAppIdentifierSettings>(configuration.GetSection(nameof(AppleAppIdentifierSettings)))
                .Configure<AndroidAppIdentifierSettings>(configuration.GetSection(nameof(AndroidAppIdentifierSettings)))
                .AddScoped<IAppleAppIdentificationService, AppleAppIdentificationService>()
                .AddScoped<IAppleAppIdentificationService, AppleAppIdentificationService>();

            return services;
        }
    }
}