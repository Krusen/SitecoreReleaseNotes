using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SitecoreReleaseNotes.Core.Extensions
{
    public static class StringExtensions
    {
        public static string ToCamelCase(this string str)
        {
            return char.ToLower(str[0]) + str.Substring(1);
        }

        public static string ReplaceIgnoreCase(this string str, string regexPattern, string replacement)
        {
            return Regex.Replace(str, regexPattern, replacement, RegexOptions.IgnoreCase);
        }

        public static string RemoveIgnoreCase(this string str, string regexPattern)
        {
            return ReplaceIgnoreCase(str, regexPattern, "");
        }
    }
}
