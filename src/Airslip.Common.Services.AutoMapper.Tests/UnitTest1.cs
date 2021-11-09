using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Services.AutoMapper.Extensions;
using Airslip.Common.Types.Enums;
using Airslip.Common.Utilities;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Xunit;

namespace Airslip.Common.Services.AutoMapper.Tests
{
    public class Tests
    {
        [Fact]
        public void Can_map_()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddAutoMapper(cfg =>
            {
                cfg.CreateMap<MyEntity, MyModel>().ReverseMap();
            });

            ServiceProvider provider = services.BuildServiceProvider() ?? 
                                        throw new NotImplementedException();

            IModelMapper<MyModel> mapper = provider.GetService<IModelMapper<MyModel>>() ?? 
                                            throw new NotImplementedException();

            MyModel myModel = new()
            {
                Id = CommonFunctions.GetId(),
                EntityId = CommonFunctions.GetId(),
                AirslipUserType = AirslipUserType.Merchant,
                EntityStatus = EntityStatus.Active
            };

            MyEntity result = mapper.Create<MyEntity>(myModel);

            result.EntityId.Should().Be(string.Empty);
            result.AirslipUserType.Should().Be(AirslipUserType.Unknown);
        }

        public class MyEntity : IEntityWithOwnership
        {
            public string Id { get; set; } = string.Empty;
            public BasicAuditInformation? AuditInformation { get; set; }
            public EntityStatus EntityStatus { get; set; }
            public string EntityId { get; set; } = string.Empty;
            public AirslipUserType AirslipUserType { get; set; }
        }
        
        public class MyModel : IModelWithOwnership
        {
            public string? Id { get; set; }
            public EntityStatus EntityStatus { get; set; }
            public string? EntityId { get; set; }
            public AirslipUserType? AirslipUserType { get; set; }
        }
    }
}