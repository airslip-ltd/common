using System.Collections.Generic;

namespace Airslip.Common.Types.Failures
{
    public class ConflictResponse : ErrorResponse
    {
        public ConflictResponse(string attribute, string value, string message)
            : base(
                "RESOURCE_EXISTS",
                message,
                new Dictionary<string, object> { { "Attribute", attribute }, { "Value", value }, { "Validation", message } })
        {

        }
    }
}
