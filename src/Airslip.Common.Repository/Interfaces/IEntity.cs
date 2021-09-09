using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;

namespace Airslip.Common.Repository.Interfaces
{
    /// <summary>
    /// A simple interface defining the common data properties for basic auditing of changes to an entity object
    /// </summary>
    public interface IEntity : IEntityWithId
    {
        BasicAuditInformation? AuditInformation { get; set; }
        
        EntityStatusEnum EntityStatus { get; set; }
    }
}