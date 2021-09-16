using Microsoft.Azure.Functions.Worker.Http;
using System.Security.Claims;

namespace Airslip.Common.Auth.Functions.Interfaces
{
    public interface IFunctionContextAccessor
    {
        HttpHeadersCollection? Headers { get; set; }
        ClaimsPrincipal? User { get; set; }
    }
}