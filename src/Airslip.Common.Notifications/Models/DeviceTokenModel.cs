using Airslip.Common.Repository.Enums;
using Airslip.Common.Repository.Interfaces;

namespace Airslip.Common.Notifications.Models
{
    public record DeviceTokenModel(
        string UserId, 
        string DeviceId, 
        string RegistrationId) : IModel
    {
        public string? Id { get; set; }
        public EntityStatus EntityStatus { get; set; }
    }
}