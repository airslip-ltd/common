using Airslip.Common.Auth.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Airslip.Common.Auth.Implementations
{
    public class HttpContextHeaderLocator : IHttpHeaderLocator
    {
        private readonly HttpContext _httpContext;

        public HttpContextHeaderLocator(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext!;
        }
        
        public string? GetValue(string headerValue, string? defaultValue = null)
        {
            if (!_httpContext.Request.Headers.ContainsKey(headerValue)) return defaultValue;
            
            return _httpContext.Request.Headers[headerValue].ToString();
        }
    }
}