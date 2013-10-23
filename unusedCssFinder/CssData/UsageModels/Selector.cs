using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ThirdParty.MostThingsWeb;
using unusedCssFinder.HtmlData;

namespace unusedCssFinder.CssData.UsageModels
{
    public class Selector
    {
        private ExCSS.Model.Selector _selector;
        private List<HtmlNode> _matchedHtmlNodes = new List<HtmlNode>();

        public Selector(ExCSS.Model.Selector selector)
        {
            _selector = selector;
        }

        public RuleSet RuleSet { get; set; }

        public bool MatchErrorOccured { get; set; }

        public List<HtmlNode> MatchedHtmlNodes { get { return _matchedHtmlNodes; } set { _matchedHtmlNodes = value; } } 

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
                if (_htmlNodeXpathes.Count == 0)
                {
                    return true;
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

        public void ApplyToHtml(HtmlDocumentWithStyles htmlDocumentWithStyles)
        {
            try
            {
                var xpath = css2xpath.Transform(this.ToString());
                var matchedNodes = htmlDocumentWithStyles.HtmlDocument.DocumentNode.SelectNodes(xpath);
                foreach (var matchedNode in matchedNodes)
                {
                    if (!MatchedHtmlNodes.Any(h => String.Equals(xpath, h.XPath, StringComparison.InvariantCulture)))
                    {
                        MatchedHtmlNodes.Add(matchedNode);
                        foreach (var declaration in RuleSet.Declarations)
                        {
                            declaration.ApplyToHtmlNode(matchedNode);
                        }
                    }
                }
            }
            catch (Exception)
            {
                MatchErrorOccured = true;
            }
        }
    }
}