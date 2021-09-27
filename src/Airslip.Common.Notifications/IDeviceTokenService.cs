using Airslip.Common.Notifications.Entities;
using Airslip.Common.Notifications.Models;
using Airslip.Common.Repository.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Airslip.Common.Notifications
{
    public interface IDeviceTokenService : IRepository<DeviceToken, DeviceTokenModel>
    {
        Task<string?> GetRegistrationId(string userId, string deviceId);
        Task<List<DeviceToken>> List();
    }
}