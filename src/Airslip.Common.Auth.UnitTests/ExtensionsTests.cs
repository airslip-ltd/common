using Airslip.Common.Auth.Enums;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Airslip.Common.Auth.Extensions;
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
            count.Should().Be(11);

        }
    }
}