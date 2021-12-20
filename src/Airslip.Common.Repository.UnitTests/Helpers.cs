using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Extensions;
using Airslip.Common.Repository.Types.Interfaces;
using Airslip.Common.Repository.Types.Models;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Threading.Tasks;

namespace Airslip.Common.Repository.UnitTests
{
    public class Helpers
    {
        public static IServiceProvider BuildRepoProvider(string? withEntityId = null, string? withUserId = null)
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
                    mock
                        .Setup(o => o.ValidateUpdate(It.IsAny<MyModel>()))
                        .Returns(Task.FromResult(new ValidationResultModel()));
                    return mock.Object;
                })
                .AddSingleton(_ =>
                {
                    Mock<IModelMapper<MyModel>> mock = new();
                    mock
                        .Setup(o => o.Create<MyEntity>(It.IsAny<MyModel>()))
                        .Returns(new MyEntity());
                    mock
                        .Setup(o => o.Create(It.IsAny<MyEntity>()))
                        .Returns(new MyModel());
                    mock
                        .Setup(o => o.Update(It.IsAny<MyModel>(), It.IsAny<MyEntity>()))
                        .Returns(new MyEntity());
                    return mock.Object;
                })
                .AddRepositories(RepositoryUserType.Manual)
                .AddScoped(_ =>
                {
                    Mock<IUserContext> mockTokenDecodeService = new();
                    mockTokenDecodeService.Setup(service => service.EntityId).Returns(withEntityId);
                    mockTokenDecodeService.Setup(service => service.UserId).Returns(withUserId);
                    mockTokenDecodeService.Setup(service => service.AirslipUserType).Returns(AirslipUserType.InternalApi);
                    return mockTokenDecodeService.Object;
                });
            
            return services.BuildServiceProvider();
        }
    }
}