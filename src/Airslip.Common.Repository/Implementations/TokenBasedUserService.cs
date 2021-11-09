using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.Implementations
{
    public class TokenBasedUserService : IRepositoryUserService
    {
        public TokenBasedUserService(ITokenDecodeService<UserToken> tokenDecodeService)
        {
            UserToken token = tokenDecodeService.GetCurrentToken();
            UserId = token.UserId;
            EntityId = token.EntityId;
            AirslipUserType = token.AirslipUserType;
        }

        public string? UserId { get; }
        public string? EntityId { get; }
        public AirslipUserType? AirslipUserType { get; }
    }
}