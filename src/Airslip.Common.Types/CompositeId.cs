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
                if (!value.Contains("|"))
                    return false;
                
                string[] splitIds = value.Split("|");

                if (splitIds.Length != 2)
                    return false;

                foreach (string splitId in splitIds)
                {
                    if (splitId.Length == 0)
                        return false;
                }

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}