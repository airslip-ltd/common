using Airslip.Common.Repository.Entities;
using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;

namespace Airslip.Common.Notifications.Entities
{
    public record DeviceToken(
        string UserId, 
        string DeviceId, 
        string RegistrationId) : IEntity
    {
        private string _id { get; set; } = string.Empty;
        public string Id
        {
            get => CompositeId.Build(UserId, DeviceId);
            set => _id = value;
        }

        public BasicAuditInformation? AuditInformation { get; set; }
        
        public EntityStatus EntityStatus { get; set; }
    }
}