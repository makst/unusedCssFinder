using System.Collections.Generic;

namespace unusedCssFinder.CssData.UsageModels
{
    public class Stylesheet
    {
        private ExCSS.Stylesheet _stylesheet;

        public Stylesheet(ExCSS.Stylesheet stylesheet)
        {
            _stylesheet = stylesheet;
        }

        public List<Directive> Directives { get; set; }
        public List<RuleSet> RuleSets { get; set; }

        public override string ToString()
        {
            return _stylesheet.ToString();
        }
    }
}