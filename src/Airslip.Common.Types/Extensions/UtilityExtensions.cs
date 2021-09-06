using System.Collections;

namespace Airslip.Common.Types.Extensions
{
    public static class UtilityExtensions
    {
        public static bool InList<TType>(this TType value, params TType[] values)
        {
            return ((IList) values).Contains(value);
        } 
    }
}