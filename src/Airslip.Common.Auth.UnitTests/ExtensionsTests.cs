using Airslip.Common.Auth.Enums;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Airslip.Common.Auth.Extensions;
using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Airslip.Common.Auth.UnitTests
{
    public class ExtensionsTests
    {
        [Fact]
        public void Can_add_services_to_service_collection()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();

            serviceCollection.AddAirslipJwtAuth(configurationBuilder.Build(), AuthType.All);

            var count = serviceCollection.Count(o => o.ServiceType.FullName.Contains("Airslip"));
            count.Should().Be(15);

        }
        
        [Fact]
        public void Can_construct_required_services()
        {
            IServiceCollection serviceCollection = new ServiceCollection();
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            serviceCollection.AddHttpContextAccessor();
            serviceCollection.AddAirslipJwtAuth(configurationBuilder.Build(), AuthType.ApiKey);

            var provider = serviceCollection.BuildServiceProvider();

            var obj1 = provider.GetService<ITokenDecodeService<ApiKeyToken>>();
            var obj2 = provider.GetService<IApiKeyRequestHandler>();

            obj1.Should().BeAssignableTo<TokenDecodeService<ApiKeyToken>>();
            obj2.Should().BeAssignableTo<ApiKeyRequestHandler>();
        }
    }
}