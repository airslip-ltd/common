using System.Security.Claims;
using System.Threading.Tasks;

namespace Airslip.Common.Auth.Interfaces
{
    public interface ITokenValidator<TExisting, TNew>
    {
        Task<ClaimsPrincipal?> GetClaimsPrincipalFromToken(string value, string forScheme, 
            string forEnvironment);
    }
}