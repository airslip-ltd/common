using Airslip.Common.Auth.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using System.Linq;
using UAParser;

namespace Airslip.Common.Auth.Implementations
{
    public class UserAgentService : IUserAgentService
    {
        private readonly HttpContext? _httpContext;

        public UserAgentService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContext = httpContextAccessor.HttpContext;
        }
        
        public string? GetRequestUserAgent()
        {
            string? result = null;
            
            if (_httpContext?.Request.Headers
                .FirstOrDefault(o => o.Key == "User-Agent") != null)
            {
                StringValues? userAgent = _httpContext?.Request.Headers["User-Agent"];
                Parser? uaParser = Parser.GetDefault();
                ClientInfo c = uaParser.Parse(userAgent);

                result = c.UA.Family + " " + c.UA.Major + "." + c.UA.Minor;
            }

            return result;
        }
    }
}