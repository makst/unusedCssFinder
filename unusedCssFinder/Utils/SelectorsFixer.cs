using System.Collections.Generic;
using unusedCssFinder.CssData.ExCssModelsWrappers;

namespace unusedCssFinder.Utils
{
    public class SelectorsFixer
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
            "after"
        };

        public Selector GetFixedSelector(Selector selector)
        {
            return selector;
        }
    }
}
