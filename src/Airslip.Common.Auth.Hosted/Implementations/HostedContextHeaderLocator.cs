using Airslip.Common.Auth.Interfaces;

namespace Airslip.Common.Auth.Hosted.Implementations
{
    public class HostedContextHeaderLocator : IHttpHeaderLocator
    {
        public string GetValue(string headerValue, string? defaultValue = null)
        {
            return string.Empty;
        }
    }
}