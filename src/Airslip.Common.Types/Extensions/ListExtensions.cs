﻿using System.Collections.Generic;

namespace Airslip.Common.Types.Extensions
{
    public static class ListExtensions
    {
        public static ICollection<T> RemoveFirst<T>(this IList<T> source)
        {
            if (source.Count > 0)
                source.RemoveAt(0);
            
            return source;
        }
        
        public static string ToCsv(this string[] values)
        {
            return string.Join(",", values);
        }
    }
}