using System.Collections.Generic;
using System.Linq;

namespace SitecoreReleaseNotes.Core.Extensions
{
    public static class EnumerableExtensions
    {
        public static IEnumerable<T> Sort<T>(this IEnumerable<T> source)
        {
            return source.OrderBy(x => x);
        }

        public static IEnumerable<T> SortDescending<T>(this IEnumerable<T> source)
        {
            return source.OrderByDescending(x => x);
        }

        public static IEnumerable<string> SortAsIntegers(this IEnumerable<string> source)
        {
            return source.OrderBy(int.Parse);
        }

        public static IEnumerable<string> SortAsIntegersDescending(this IEnumerable<string> source)
        {
            return source.OrderByDescending(int.Parse);
        }
    }
}
