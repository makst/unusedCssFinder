using System;
using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;
using unusedCssFinder.Models;
using unusedCssFinder.Models.Html;
using unusedCssFinder.Utils;

namespace unusedCssFinder.CssData.UsageModels
{
    public class Declaration
    {
        private ExCSS.Model.Declaration _declaration;
        private Dictionary<HtmlNodeModel, DeclarationUsageType> _htmlNodeUsageTypes = new Dictionary<HtmlNodeModel, DeclarationUsageType>();

        public Declaration(ExCSS.Model.Declaration declaration)
        {
            _declaration = declaration;
        }

        public RuleSet RuleSet { get; set; }
        public string Name { get { return _declaration.Name; } }
        public bool Important { get { return _declaration.Important; } }
        public Expression Expression { get { return _declaration.Expression; } }

        public Dictionary<HtmlNodeModel, DeclarationUsageType> HtmlNodeUsageTypes
        {
            get
            {
                return _htmlNodeUsageTypes;
            }
            set
            {
                _htmlNodeUsageTypes = value;
            }
        }

        public bool IsNotUsed
        {
            get
            {
                if (_htmlNodeUsageTypes.Keys.Count == 0)
                {
                    return true;
                }
                if (_htmlNodeUsageTypes.Values.All(v => v == DeclarationUsageType.NotUsed))
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
                if (_htmlNodeUsageTypes.Values.All(v => v == DeclarationUsageType.Overriden))
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsNotUsedOnPage(HtmlPageModel htmlPage)
        {
            if (_htmlNodeUsageTypes.Keys.Count(n => ReferenceEquals(n.HtmlPage, htmlPage)) == 0)
            {
                return true;
            }
            if (_htmlNodeUsageTypes.Where(x => ReferenceEquals(x.Key.HtmlPage, htmlPage)).Select(d => d.Value).All(v => v == DeclarationUsageType.NotUsed))
            {
                return true;
            }
            return false;
        }

        public bool IsOverridenOnPage(HtmlPageModel htmlPage)
        {
            if (IsNotUsedOnPage(htmlPage))
            {
                return false;
            }
            if (_htmlNodeUsageTypes.Where(x => ReferenceEquals(x.Key.HtmlPage, htmlPage)).Select(d => d.Value).All(v => v == DeclarationUsageType.Overriden))
            {
                return true;
            }
            return false;
        }

        public void TryToOverrideBy(HtmlNodeModel htmlNodeModel, Declaration newDeclaration)
        {
            var nodeExists = HtmlNodeUsageTypes.Keys.Contains(htmlNodeModel);
            if (!nodeExists)
            {
                return;
            }
            if (string.Equals(Name, newDeclaration.Name, StringComparison.InvariantCultureIgnoreCase))
            {
                if (Important)
                {
                    return;
                }
                if (newDeclaration.RuleSet.Selector.Specificity > RuleSet.Selector.Specificity )
                {
                    var newSelectorFixed = SelectorsFixer.GetFixedSelector(newDeclaration.RuleSet.Selector);
                    var selectorFixed = SelectorsFixer.GetFixedSelector(RuleSet.Selector);
                    if (!string.Equals(newSelectorFixed, selectorFixed, StringComparison.InvariantCulture))
                    {
                        HtmlNodeUsageTypes[htmlNodeModel] = DeclarationUsageType.Overriden;
                    }
                }
            }
        }

        public void ApplyToHtmlNode(HtmlNodeModel htmlNodeModel)
        {
            var nodeExists = HtmlNodeUsageTypes.ContainsKey(htmlNodeModel);
            if (nodeExists)
            {
                throw new ArgumentException("html node is currently in use");
            }
            HtmlNodeUsageTypes.Add(htmlNodeModel, DeclarationUsageType.Used);
        }

        public override string ToString()
        {
            return _declaration.ToString();
        }
    }
}