using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.UnitTests.Helpers;
using FluentAssertions;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class UserAgentServiceTests
    {
        [Fact]
        public void Can_decode_user_agent()
        {
            IUserAgentService service = HelperFunctions
                .GenerateUserAgentService(withUserAgent: Constants.UA_WINDOWS_10_EDGE);

            string userAgent = service.GetRequestUserAgent();

            userAgent.Should().NotBeNull();
            userAgent.Should().Be(Constants.UA_WINDOWS_10_EDGE_MATCH);
        }
        
        [Fact]
        public void Returns_null_with_no_user_agent()
        {
            IUserAgentService service = HelperFunctions
                .GenerateUserAgentService(withUserAgent: null);

            string userAgent = service.GetRequestUserAgent();

            userAgent.Should().BeNull();
        }
    }
}