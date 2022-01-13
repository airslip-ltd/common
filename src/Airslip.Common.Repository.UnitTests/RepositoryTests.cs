using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Implementations;
using Airslip.Common.Repository.Types.Enums;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Repository.UnitTests.Common;
using Airslip.Common.Types.Interfaces;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Airslip.Common.Repository.UnitTests;

public class RepositoryTests
{
    [Fact]
    public async Task Error_is_not_thrown_when_unknown_resource_is_deleted()
    {
        IServiceProvider provider = Helpers.BuildRepoProvider();

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