using System.Collections.Generic;

namespace UnusedCssFinder.CssData
{
    public class Stylesheet : ExCSS.Stylesheet
    {
        public new List<Directive> Directives { get; set; }
        public new List<RuleSet> RuleSets { get; set; }
        public Dictionary<int, string> MatchedXpathes { get; set; }
    }
}