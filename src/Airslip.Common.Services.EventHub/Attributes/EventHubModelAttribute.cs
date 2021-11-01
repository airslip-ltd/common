using System;

namespace Airslip.Common.Services.EventHub.Attributes
{
    [AttributeUsage(AttributeTargets.Class)]  
    public class EventHubModelAttribute : Attribute
    {
        public EventHubModelAttribute(string eventHubName)
        {
            EventHubName = eventHubName;
        }
        
        public string EventHubName { get; }
    }
}