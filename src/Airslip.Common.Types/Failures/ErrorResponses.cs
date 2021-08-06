using Airslip.Common.Contracts;
using System.Collections.Generic;

namespace Airslip.Common.Types.Failures
{
    public class ErrorResponses : IResponse
    {
        public static readonly ErrorResponses Empty = new();

        private ErrorResponses()
        {
            Errors = new List<ErrorResponse>();
        }

        public ICollection<ErrorResponse> Errors { get; }

        public ErrorResponses(ICollection<ErrorResponse> errors)
        {
            Errors = errors;
        }
    }
}