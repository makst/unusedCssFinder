using System.Collections.Generic;

namespace UnusedCssFinder.CssData
{
    public class RuleSet : ExCSS.Model.RuleSet
    {
        public new List<Declaration> Declarations { get; set; }
        public new List<Selector> Selectors { get; set; }
    }
}