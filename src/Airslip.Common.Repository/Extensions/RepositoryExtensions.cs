using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            Services.ConfigureServices(services);

            return services;
        }
    }
}