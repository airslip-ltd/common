using Airslip.Common.Contracts;
using System.Collections.Generic;

namespace Airslip.Common.Types.Failures
{
    public class ErrorResponses : IResponse
    {
        public IEnumerable<ErrorResponse> Errors { get; }

        public ErrorResponses(IEnumerable<ErrorResponse> errors)
        {
            Errors = errors;
        }
    }
}
