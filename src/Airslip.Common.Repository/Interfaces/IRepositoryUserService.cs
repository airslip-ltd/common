using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.Interfaces
{
    public interface IRepositoryUserService
    {
        string? UserId { get; }
        string? EntityId { get; }
        
        AirslipUserType? AirslipUserType { get; }
    }
}