using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Interfaces
{
    public interface IApiKeyRequestHandler
    {
        Task<AuthenticateResult> Handle(HttpRequest request);
    }
}