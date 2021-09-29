using Airslip.Common.Types.Extensions;
using System;
using System.Text;

namespace Airslip.Common.Types
{
    public static class StorageFunctions
    {
        public static string BuildBlobName(params string[] values)
        {
            if(values.Length.Equals(0))
                return string.Empty;
            
            StringBuilder sb = new();
            foreach (string value in values)
                sb.Append($"/{value.ToKebabCasing()}");

            return sb.ToString()[1..];
        }
    }
}