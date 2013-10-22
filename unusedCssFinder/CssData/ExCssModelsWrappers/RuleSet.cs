using System.Collections.Generic;
using ExCSS.Model;

namespace unusedCssFinder.CssData.ExCssModelsWrappers
{
    public class RuleSet
    {
        private ExCSS.Model.RuleSet _ruleSet;

        public RuleSet(ExCSS.Model.RuleSet ruleSet)
        {
            _ruleSet = ruleSet;
        }

        public new List<Declaration> Declarations { get { return _ruleSet.Declarations; } }
        public new List<Selector> Selectors { get; set; }

        public override string ToString()
        {
            return _ruleSet.ToString();
        }
    }
}