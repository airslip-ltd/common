using System.Collections.Generic;

namespace Airslip.Common.Types.Failures
{
    public class NotFoundResponse : ErrorResponse
    {
        public NotFoundResponse(string attribute, string value, string message)
            : base(
                "RESOURCE_NOT_FOUND",
                message,
                new Dictionary<string, object> { { "Attribute", attribute }, { "Value", value }, { "Validation", message } })
        {

        }
    }
}
