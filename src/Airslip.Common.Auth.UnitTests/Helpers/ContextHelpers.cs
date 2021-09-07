using Airslip.Common.Auth.Schemes;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Security.Claims;

namespace Airslip.Common.Auth.UnitTests.Helpers
{
    public static class ContextHelpers
    {
        public static Mock<IHttpContextAccessor> GenerateContextWithBearerToken(string bearerToken, 
            string userAgent = Constants.UA_WINDOWS_10_EDGE,
            ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            context.Request.Headers["Authorization"] = $"Bearer {bearerToken}";
            context.Request.Headers["User-Agent"] = userAgent;
            if (withClaimsPrincipal != null) context.User = withClaimsPrincipal;
            
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }
        
        public static Mock<IHttpContextAccessor> GenerateContextWithApiKey(string apiKey, ClaimsPrincipal withClaimsPrincipal = null)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            context.Request.Headers[ApiKeyAuthenticationSchemeOptions.ApiKeyHeaderField] = apiKey;
            if (withClaimsPrincipal != null) context.User = withClaimsPrincipal;
            
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }
        
        public static Mock<IHttpContextAccessor> GenerateContextWithForwarder(string forwarder = null, 
            string remoteAddr = null)
        {
            Mock<IHttpContextAccessor> mockHttpContextAccessor = new();
            DefaultHttpContext context = new();
            
            if(forwarder != null) context.Request.Headers["X-Forwarded-For"] = forwarder;
            if(remoteAddr != null) context.Request.Headers["REMOTE_ADDR"] = remoteAddr;
            
            mockHttpContextAccessor.Setup(_ => _.HttpContext).Returns(context);
            
            return mockHttpContextAccessor;
        }
    }
}