using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Types.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Repository.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services, 
        RepositoryUserType repositoryUserType = RepositoryUserType.TokenBased)
    {
        Services.ConfigureServices(services, repositoryUserType);

        return services;
    }
    
    public static IServiceCollection AddEntitySearch(this IServiceCollection services)
    {
        services.AddScoped(typeof(IEntitySearch<>), typeof(EntitySearch<>));

        return services;
    }
}