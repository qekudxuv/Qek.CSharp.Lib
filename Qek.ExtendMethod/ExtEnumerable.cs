using System.Collections.Generic;
using System.Linq;

namespace Qek.ExtendMethod
{
    public static class ExtEnumerable
    {
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> source)
        {
            return source == null || !source.Any();
        }
    }
}
