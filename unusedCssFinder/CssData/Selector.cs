using System.Collections.Generic;
using UnusedCssFinder.Utils;

namespace UnusedCssFinder.CssData
{
    public class Selector : ExCSS.Model.Selector
    {
        public new List<SimpleSelector> SimpleSelectors { get; set; }
        public Specificity Specificity
        {
            get
            {
                var res = new Specificity(0, 0, 0, 0);
                foreach (var simpleSelector in SimpleSelectors)
                {
                    res += simpleSelector.Specificity;
                }
                return res;
            }
        }
    }
}