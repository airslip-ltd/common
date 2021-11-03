using Airslip.Common.Repository.Enums;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, 
            RepositoryUserType repositoryUserType = RepositoryUserType.TokenBased)
        {
            Services.ConfigureServices(services, repositoryUserType);

            return services;
        }
    }
}