using Airslip.Common.Auth.Implementations;
using Airslip.Common.Auth.Interfaces;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using Xunit;

namespace Airslip.Common.Auth.UnitTests
{
    public class HttpHeaderLocatorTests
    {
        [Fact]
        public void Can_decode_null_httpcontext()
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            IHttpHeaderLocator locator = new HttpContextHeaderLocator(mockHttpContextAccessor.Object);

            locator.GetValue("Hello", "MyDefault").Should().Be("MyDefault");
        }
        
        [Fact]
        public void Can_decode_not_null_httpcontext()
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            context.Request.Headers["Authorization"] = "Bearer SomeToken";

            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            IHttpHeaderLocator locator = new HttpContextHeaderLocator(mockHttpContextAccessor.Object);

            locator.GetValue("Authorization", "MyDefault").Should().Be("Bearer SomeToken");
        }
    }
}