using System.Collections.Generic;

namespace unusedCssFinder.CssData.UsageModels
{
    public class RuleSet
    {
        private ExCSS.Model.RuleSet _ruleSet;

        public RuleSet(ExCSS.Model.RuleSet ruleSet)
        {
            _ruleSet = ruleSet;
        }

        public List<Declaration> Declarations { get; set; }
        public List<Selector> Selectors { get; set; }

        public override string ToString()
        {
            return _ruleSet.ToString();
        }
    }
}