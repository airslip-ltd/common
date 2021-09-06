using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Interfaces
{
    public interface IApiKeyValidator
    {
        Task<ClaimsPrincipal?> IsApiKeyTokenValid(string value);
    }
}