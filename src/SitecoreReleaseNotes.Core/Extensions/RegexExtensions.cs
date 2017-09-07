using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SitecoreReleaseNotes.Core.Extensions
{
    public static class RegexExtensions
    {
        public static IEnumerable<T> Select<T>(this MatchCollection matches, Func<Match, T> selector)
        {
            foreach (Match match in matches)
            {
                yield return selector(match);
            }
        }
    }
}
