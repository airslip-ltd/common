using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.AutoMapper.Extensions;
using Airslip.Common.Services.AutoMapper.Implementations;
using Airslip.Common.Utilities.Extensions;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Airslip.Common.Services.AutoMapper
{
    public static class Services
    {
        public static void ConfigureServices(IServiceCollection serviceCollection, Action<MapperConfigurationExpression> mapperConfiguration)
        {
            serviceCollection.AddSingleton(typeof(IModelMapper<>), typeof(AutoMapperModelMapper<>));

            MapperConfigurationExpression configExpression = new();
            configExpression.IgnoreUnmapped();
            mapperConfiguration(configExpression);
            configExpression.AddProfile(new IgnoreOwnershipProfile());
            
            // Auto Mapper Configurations
            MapperConfiguration mappingConfig = new(configExpression);
            

            mappingConfig.AssertConfigurationIsValid();
            
            IMapper? mapper = mappingConfig.CreateMapper();
            serviceCollection.AddSingleton(mapper);
        }
        
        public class IgnoreOwnershipProfile : Profile
        {
            public IgnoreOwnershipProfile()
            {
                // If IEntityWithOwnership - ignore names EnityId AirslipUserType
                // If IModelWithOwnership - Allow through
                ShouldMapProperty = p =>
                {
                    return !(typeof(IModelWithOwnership)
                                 .IsAssignableFrom(p.DeclaringType)
                             &&
                             p.Name.InList(
                                 nameof(IModelWithOwnership.EntityId),
                                 nameof(IModelWithOwnership.AirslipUserType)));
                };
            }
        }
    }
}