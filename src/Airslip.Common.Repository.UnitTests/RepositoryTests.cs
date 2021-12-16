using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Repository.Models;
using Airslip.Common.Repository.Types.Entities;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Xunit;
using IModel = Airslip.Common.Repository.Interfaces.IModel;

namespace Airslip.Common.Repository.UnitTests
{
    public class RepositoryTests
    {
        [Fact]
        public async Task Error_is_not_thrown_when_unknown_resource_is_deleted()
        {
            IServiceCollection services = new ServiceCollection();

            services
                .AddSingleton(_ =>
                {
                    Mock<IContext> mock = new();
                    return mock.Object;
                })
                .AddSingleton(_ =>
                {
                    Mock<IModelValidator<MyModel>> mock = new();
                    return mock.Object;
                })
                .AddSingleton(_ =>
                {
                    Mock<IModelMapper<MyModel>> mock = new();
                    return mock.Object;
                })
                .AddRepositories(RepositoryUserType.Manual)
                .AddScoped(_ =>
                {
                    Mock<IUserContext> mockTokenDecodeService = new();
                    mockTokenDecodeService.Setup(service => service.EntityId).Returns((string?)null);
                    mockTokenDecodeService.Setup(service => service.UserId).Returns((string?)null);
                    mockTokenDecodeService.Setup(service => service.AirslipUserType).Returns((AirslipUserType?)null);
                    return mockTokenDecodeService.Object;
                });
            IServiceProvider provider = services.BuildServiceProvider();

            IRepository<MyEntity, MyModel> repo = provider.GetService<IRepository<MyEntity, MyModel>>()
               ?? throw new NotImplementedException();

            RepositoryActionResultModel<MyModel> delete = await repo.Delete("unknown-id");

            delete.Should().BeOfType<FailedActionResultModel<MyModel>>();
            delete.ResultType.Should().Be(ResultType.NotFound);
        }
        
        [Fact]
        public void Can_construct_repository_with_no_delivery_service()
        {
            Mock<IContext> mockContext = new();
            Mock<IModelValidator<MyModel>> mockModelValidator = new();
            Mock<IModelMapper<MyModel>> mockModelMapper = new();
            Mock<IUserContext> mockTokenDecodeService = new();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(mockModelValidator.Object);
            serviceCollection.AddSingleton(mockModelMapper.Object);
            serviceCollection.AddSingleton(mockContext.Object);
            serviceCollection.AddSingleton(mockTokenDecodeService.Object);
            serviceCollection.AddSingleton(mockTokenDecodeService.Object);
            serviceCollection.AddRepositories(RepositoryUserType.Manual);
            
            var provider = serviceCollection.BuildServiceProvider();
            var myRepo = provider.GetService<IRepository<MyEntity, MyModel>>();

            myRepo.Should().NotBeNull();
        }
        
        [Fact]
        public void Can_construct_repository_with_delivery_service()
        {
            Mock<IContext> mockContext = new();
            Mock<IModelValidator<MyModel>> mockModelValidator = new();
            Mock<IModelMapper<MyModel>> mockModelMapper = new();
            Mock<IUserContext> mockTokenDecodeService = new();

            IServiceCollection serviceCollection = new ServiceCollection();

            serviceCollection.AddSingleton(mockModelValidator.Object);
            serviceCollection.AddSingleton(mockModelMapper.Object);
            serviceCollection.AddSingleton(mockContext.Object);
            serviceCollection.AddSingleton(mockTokenDecodeService.Object);
            serviceCollection.AddSingleton(mockTokenDecodeService.Object);
            serviceCollection.AddSingleton(typeof(IModelDeliveryService<>), 
                typeof(NullModelDeliveryService<>));
            serviceCollection.AddRepositories(RepositoryUserType.Manual);
            
            var provider = serviceCollection.BuildServiceProvider();
            var myRepo = provider.GetService<IRepository<MyEntity, MyModel>>();

            myRepo.Should().NotBeNull();
        }
    }

    public class MyEntity : IEntity
    {
        public string Id { get; set; } = string.Empty;
        public BasicAuditInformation? AuditInformation { get; set; }
        public EntityStatus EntityStatus { get; set; }
    }


    public class TTokenType : IDecodeToken
    {
        public void SetCustomClaims(List<Claim> tokenClaims, TokenEncryptionSettings settings)
        {
            throw new NotImplementedException();
        }

        public string TokenType { get; init; } = string.Empty;
        public bool? IsAuthenticated { get; init; }
        public string CorrelationId { get; init; }= string.Empty;
        public string IpAddress { get; init; }= string.Empty;
        public string BearerToken { get; init; }= string.Empty;
        public string UserAgent { get; init; }= string.Empty;
        public string EntityId { get; init; }= string.Empty;
        public AirslipUserType AirslipUserType { get; init; }
        public string Environment { get; init; }= string.Empty;
    }

    public class MyModel : IModel
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
    }
}