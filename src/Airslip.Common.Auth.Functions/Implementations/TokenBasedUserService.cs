using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class ApiKeyTokenUserService : IRepositoryUserService
    {
        public ApiKeyTokenUserService(ITokenDecodeService<ApiKeyToken> tokenDecodeService)
        {
            ApiKeyToken token = tokenDecodeService.GetCurrentToken();
            UserId = null;
            EntityId = token.EntityId;
            AirslipUserType = token.AirslipUserType;
        }

        public string? UserId { get; }
        public string? EntityId { get; }
        public AirslipUserType? AirslipUserType { get; }
    }
}