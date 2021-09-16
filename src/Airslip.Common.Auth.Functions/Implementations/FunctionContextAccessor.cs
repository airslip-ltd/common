using Airslip.Common.Auth.Functions.Interfaces;
using Microsoft.Azure.Functions.Worker.Http;
using System.Security.Claims;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class FunctionContextAccessor : IFunctionContextAccessor
    {
        public HttpHeadersCollection? Headers { get; set; }
        public ClaimsPrincipal? User { get; set; }
    }
}