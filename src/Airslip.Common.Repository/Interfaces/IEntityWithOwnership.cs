using Airslip.Common.Types.Enums;

namespace Airslip.Common.Repository.Interfaces
{
    /// <summary>
    /// A simple interface defining the common data properties for basic auditing of changes to an entity object
    /// </summary>
    public interface IEntityWithOwnership : IEntity
    {
        string EntityId { get; set; }
        
        AirslipUserType AirslipUserType { get; set; }
    }
}