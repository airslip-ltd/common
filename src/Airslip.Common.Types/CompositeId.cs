using System;
using System.Linq;

namespace Airslip.Common.Types
{
    public static class CompositeId
    {
        public static string Build(params string[] keys)
        {
            if (keys.Length == 0)
                return string.Empty;

            string result = keys.Aggregate(
                string.Empty,
                (current, key) => current + $"|{key}");

            return result[1..];
        }
        
        public static bool CheckIsComposite(string value)
        {
            try
            {
                string[] splitIds = value.Split("|");

                return splitIds.Select(splitId => splitId.Length > 0).FirstOrDefault();
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}