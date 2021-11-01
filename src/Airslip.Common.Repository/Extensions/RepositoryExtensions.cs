using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Airslip.Common.Repository.Extensions
{
    public static class RepositoryExtensions
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, 
            RepositoryUserType repositoryUserType = RepositoryUserType.TokenBased)
        {
            return services.AddRepositories(typeof(NullModelDeliveryService<>), repositoryUserType);
        }
        
        public static IServiceCollection AddRepositories(this IServiceCollection services, 
            Type modelDeliveryServiceType,
            RepositoryUserType repositoryUserType = RepositoryUserType.TokenBased)
        {
            Services.ConfigureServices(services, modelDeliveryServiceType, repositoryUserType);

            return services;
        }
    }
}