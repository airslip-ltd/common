using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Implementations.Events.Entity;
using Airslip.Common.Repository.Implementations.Events.Model;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Repository
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, 
            RepositoryUserType repositoryUserType)
        {
            serviceCollection
                .AddScoped(typeof(IRepositoryLifecycle<,>), typeof(RepositoryLifecycle<,>))
                .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
                .AddScoped(typeof(IModelPostProcessEvent<>), typeof(ModelDeliveryEvent<>))
                .AddScoped(typeof(IModelPostProcessEvent<>), typeof(ModelFormatEvent<>))
                .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityBasicAuditEvent<>))
                .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityOwnershipEvent<>))
                .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityStatusEvent<>))
                .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityDefaultIdEvent<>));

            switch (repositoryUserType)
            {
                case RepositoryUserType.Null:
                    serviceCollection.AddSingleton<IUserContext, NullUserService>();
                    break;
                case RepositoryUserType.TokenBased:
                    serviceCollection.AddScoped<IUserContext, TokenBasedUserService>();
                    break;
                case RepositoryUserType.Manual:
                    // Inject nothing, let the developer do it...
                    break;
            }
        }
    }
}