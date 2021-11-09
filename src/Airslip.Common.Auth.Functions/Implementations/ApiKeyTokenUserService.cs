using Airslip.Common.Auth.Interfaces;
using Airslip.Common.Auth.Models;
using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Auth.Functions.Implementations
{
    public class ApiKeyTokenUserService : IRepositoryUserService
    {
        private readonly ITokenDecodeService<ApiKeyToken> _tokenDecodeService;

        public ApiKeyTokenUserService(ITokenDecodeService<ApiKeyToken> tokenDecodeService)
        {
            _tokenDecodeService = tokenDecodeService;
        }

        public string? UserId => null;

        public string? EntityId => _tokenDecodeService.GetCurrentToken().EntityId;

        public AirslipUserType? AirslipUserType => _tokenDecodeService.GetCurrentToken().AirslipUserType;
    }
}