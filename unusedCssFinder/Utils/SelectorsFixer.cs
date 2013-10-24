using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using unusedCssFinder.CssData.UsageModels;

namespace unusedCssFinder.Utils
{
    public static class SelectorsFixer
    {
        private static List<string> _browserDependentPseudo = new List<string>
        {
            "link",
            "visited",
            "active",
            "hover",
            "focus",
            "first-letter",
            "first-line",
            "before",
            "after",
            "target",
            "enabled",
            "disabled",
            "checked"
        };

        public static string GetFixedSelector(Selector selector)
        {
            return Regex.Replace(selector.ToString(), @"((\:{1,2})([\w-]+))", new MatchEvaluator((Match m) =>
            {
                if (string.Equals(m.Groups[2].Value /* : or ::*/ , "::", StringComparison.InvariantCulture))
                {
                    return string.Empty;
                }
                if (_browserDependentPseudo.Any(p => string.Equals(p, m.Groups[3].Value /*pseudo*/, StringComparison.InvariantCulture)))
                {
                    return string.Empty;
                }
                return m.Groups[1].Value; /* : or :: with pseudo */
            }));
        }
    }
}
