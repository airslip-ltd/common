using System.Collections.Generic;

namespace Airslip.Common.Types
{
    public static class ListExtensions
    {
        public static ICollection<T> RemoveFirst<T>(this IList<T> source)
        {
            if (source.Count > 0)
                source.RemoveAt(0);
            
            return source;
        }
    }
}