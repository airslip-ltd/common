using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Interfaces;

namespace Airslip.Common.Repository.Implementations
{
    public class TokenBasedUserService : IRepositoryUserService
    {
        public TokenBasedUserService(ITokenDecodeService<UserToken> tokenDecodeService)
        {
            UserId = tokenDecodeService.GetCurrentToken().UserId;
        }

        public string? UserId { get; }
    }
}