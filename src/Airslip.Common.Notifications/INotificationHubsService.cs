using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Airslip.Common.Notifications
{
    public interface INotificationHubsService
    {
        Task<string> CreateRegistrationId();
        Task<string> CreateRegistration(
            DeviceTypes deviceType,
            string deviceToken,
            string deviceId,
            string userId,
            ICollection<string> tags);
        Task Send(
            DeviceTypes deviceType,
            string message,
            IEnumerable<string>? tags,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default);
        Task<string> Schedule(
            DeviceTypes deviceType,
            long scheduledTime,
            string message,
            IEnumerable<string> tags,
            Dictionary<string, string>? headers = null,
            CancellationToken cancellationToken = default);
        Task Cancel(string scheduledNotificationId);
        Task DeleteRegistration(string userId, string deviceId);
        Task<IEnumerable<NotificationRegistration>> GetAllRegistrations(int top);
        Task<IEnumerable<NotificationRegistration>> GetRegistrationsByChannel(string deviceToken, int top);
        Task<IEnumerable<NotificationRegistration>> GetRegistrationsByTag(string tag, int top);
    }
}