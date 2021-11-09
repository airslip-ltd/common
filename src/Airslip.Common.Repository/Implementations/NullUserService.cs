using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.Implementations
{
    public class NullUserService : IRepositoryUserService
    {
        public string? UserId => null;
        public string? EntityId => null;
        public AirslipUserType? AirslipUserType => null;
    }
}