using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Implementations.Events.Entity;
using Airslip.Common.Repository.Implementations.Events.Entity.PreProcess;
using Airslip.Common.Repository.Implementations.Events.Entity.PreValidate;
using Airslip.Common.Repository.Implementations.Events.Model;
using Airslip.Common.Repository.Implementations.Events.Model.PostProcess;
using Airslip.Common.Repository.Implementations.Events.Model.PreProcess;
using Airslip.Common.Repository.Implementations.Events.Model.PreValidate;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Airslip.Common.Repository;

public static class Services
{
    internal static void ConfigureServices(IServiceCollection serviceCollection, 
        RepositoryUserType repositoryUserType)
    {
        serviceCollection
            .AddScoped(typeof(IRepositoryLifecycle<,>), typeof(RepositoryLifecycle<,>))
            .AddScoped(typeof(IRepository<,>), typeof(Repository<,>))
            .AddScoped(typeof(IModelPostProcessEvent<>), typeof(ModelDeliveryEvent<>))
            .AddScoped(typeof(IModelPostProcessEvent<>), typeof(ModelFormatEvent<>))
            .AddScoped(typeof(IModelPreProcessEvent<>), typeof(ModelTimeStampEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityBasicAuditEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityOwnershipEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityStatusEvent<>))
            .AddScoped(typeof(IEntityPreProcessEvent<>), typeof(EntityDefaultIdEvent<>))
            .AddScoped(typeof(IEntityPreValidateEvent<,>), typeof(EntityFoundValidation<,>))
            .AddScoped(typeof(IEntityPreValidateEvent<,>), typeof(EntityStatusValidation<,>))
            .AddScoped(typeof(IEntityPreValidateEvent<,>), typeof(EntityTimelineValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(ModelCreateValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(ModelIdValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(ModelUpdateValidation<,>))
            .AddScoped(typeof(IModelPreValidateEvent<,>), typeof(IdRequiredValidation<,>));
            
        switch (repositoryUserType)
        {
            case RepositoryUserType.Null:
                serviceCollection.AddSingleton<IUserContext, NullUserContext>();
                break;
            case RepositoryUserType.TokenBased:
                serviceCollection
                    .AddScoped<IUserContext, TokenBasedUserContext>()
                    .AddScoped(typeof(IEntityPreValidateEvent<,>), 
                        typeof(EntityOwnershipValidationEvent<,>));
                break;
            case RepositoryUserType.Service:
                serviceCollection.AddScoped<IUserContext, ServiceUserContext>();
                break;
            case RepositoryUserType.Manual:
                // Inject nothing, let the developer do it...
                break;
        }
    }
}