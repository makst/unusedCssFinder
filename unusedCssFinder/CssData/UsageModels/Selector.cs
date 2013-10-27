using System.Collections.Generic;
using System.Linq;
using unusedCssFinder.Models.Html;

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

        public bool IsNotUsedOnPage(HtmlPageModel htmlPage)
        {
            if (MatchErrorOccured)
            {
                return false;
            }
            if (RuleSet.Declarations.All(d => d.IsNotUsedOnPage(htmlPage)))
            {
                return true;
            }
            return false;
        }

        public bool IsOverridenOnPage(HtmlPageModel htmlPage)
        {
            if (MatchErrorOccured)
            {
                return false;
            }
            if (IsNotUsedOnPage(htmlPage))
            {
                return false;
            }
            if (RuleSet.Declarations.All(d => d.IsOverridenOnPage(htmlPage)))
            {
                return true;
            }
            return false;
        }
    }
}