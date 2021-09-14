using Airslip.Common.Types;
using System.Collections.Generic;

namespace Airslip.Common.Notifications
{
    public interface INotificationCommon
    {
        ICollection<string> CreateEmptyTags();
        string BuildUserIdTag(string userId);
        ICollection<string> CreateTagsWithUserId(string userId);
        bool CheckUserIdTagExists(IEnumerable<string> tags);
        string BuildNotificationBody(DeviceTypes deviceType, string message);
    }
}