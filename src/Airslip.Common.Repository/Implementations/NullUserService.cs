using Airslip.Common.Repository.Interfaces;
using Airslip.Common.Types.Enums;
using Airslip.Common.Types.Interfaces;

namespace Airslip.Common.Repository.Implementations
{
    public class NullUserService : IUserContext
    {
        public string? UserId => null;
        public string? EntityId => null;
        public AirslipUserType? AirslipUserType => null;
    }
}