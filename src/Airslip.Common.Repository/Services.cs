using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Airslip.Common.Repository
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, Type modelDeliveryServiceType,
            RepositoryUserType repositoryUserType)
        {
            serviceCollection
                .AddSingleton(typeof(IModelDeliveryService<>), modelDeliveryServiceType)
                .AddScoped(typeof(IRepository<,>), typeof(Repository<,>));

            switch (repositoryUserType)
            {
                case RepositoryUserType.Null:
                    serviceCollection.AddSingleton<IRepositoryUserService, NullUserService>();
                    break;
                case RepositoryUserType.TokenBased:
                    serviceCollection.AddScoped<IRepositoryUserService, TokenBasedUserService>();
                    break;
                case RepositoryUserType.Manual:
                    // Inject nothing, let the developer do it...
                    break;
            }
        }
    }
}