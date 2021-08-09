﻿using Airslip.Common.Contracts;
using Airslip.Common.Types.Hateoas;
using System.Collections.Generic;
using System.Linq;

namespace Airslip.Common.Types.Failures
{
    public class ErrorResponse : ErrorLinkResourceBase, IFail
    {
        public string ErrorCode { get; }
        public string? Message { get; }
        public IDictionary<string, object> Metadata { get; }

        public ErrorResponse(string errorCode, string? messageTemplate = null, IDictionary<string, object>? metadata = null)
        {
            Message = metadata == null
                ? messageTemplate
                : metadata.Aggregate(messageTemplate,
                    (current, pair) => current?.Replace($"{{{pair.Key}}}", $"{pair.Value}"));
            ErrorCode = errorCode;
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        public ErrorResponse Add(string key, object value)
        {
            Metadata.Add(key, value);
            return this;
        }
    }
}