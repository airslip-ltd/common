using Airslip.Common.Types;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Notifications
{
    public class NotificationCommon : INotificationCommon
    {
        public ICollection<string> CreateEmptyTags() => new List<string>();

        public string BuildUserIdTag(string userId) => $"userid:{userId}";

        public ICollection<string> CreateTagsWithUserId(string userId)
        {
            ICollection<string> tags = CreateEmptyTags();
            tags.Add(BuildUserIdTag(userId));
            return tags;
        }

        public bool CheckUserIdTagExists(IEnumerable<string> tags)
        {
            return tags.Any(s => s.Contains("userid:"));
        }
        
        public string BuildNotificationBody(DeviceTypes deviceType, string message)
        {
            switch (deviceType)
            {
                case DeviceTypes.APPLE:
                    AppleNotificationPayload appleNotificationPayload = new(message);
                    return Json.Serialize(appleNotificationPayload);
                case DeviceTypes.ANDROID:
                    AndroidNotificationPayload androidNotificationPayload = new(message);
                    return Json.Serialize(androidNotificationPayload);
                case DeviceTypes.NONE:
                    throw new InvalidOperationException(NotificationConstants.UnsupportedMessage("apple", "android"));
                default:
                    throw new InvalidOperationException(NotificationConstants.UnsupportedMessage("apple", "android"));
            }
        }
    }
}