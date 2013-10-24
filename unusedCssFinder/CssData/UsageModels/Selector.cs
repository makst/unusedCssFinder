using System.Collections.Generic;
using System.Linq;

namespace unusedCssFinder.CssData.UsageModels
{
    public class Selector
    {
        private ExCSS.Model.Selector _selector;

        public Selector(ExCSS.Model.Selector selector)
        {
            _selector = selector;
        }

        public RuleSet RuleSet { get; set; }

        public bool MatchErrorOccured { get; set; }

        public List<SimpleSelector> SimpleSelectors { get; set; }

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

        public override string ToString()
        {
            return _selector.ToString();
        }

        public bool IsNotUsed
        {
            get
            {
                if (MatchErrorOccured)
                {
                    return false;
                }
                if (RuleSet.Declarations.All(d => d.IsNotUsed))
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsOverriden
        {
            get
            {
                if (MatchErrorOccured)
                {
                    return false;
                }
                if (IsNotUsed)
                {
                    return false;
                }
                if (RuleSet.Declarations.All(d => d.IsOverriden))
                {
                    return true;
                }
                return false;
            }
        }
    }
}