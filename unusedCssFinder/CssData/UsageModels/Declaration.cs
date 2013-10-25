using System;
using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;
using unusedCssFinder.Models;

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
            if (_htmlNodeUsageTypes.Keys.Count(n => n.HtmlPage == htmlPage) == 0)
            {
                return true;
            }
            if (_htmlNodeUsageTypes.Where(x => x.Key.HtmlPage == htmlPage).Select(d => d.Value).All(v => v == DeclarationUsageType.NotUsed))
            {
                return true;
            }
            return false;
        }

        public bool IsOverridenOnPage(HtmlPageModel htmlPage)
        {
            if (IsNotUsedOnPage(htmlPage))
            {
                return true;
            }
            if (_htmlNodeUsageTypes.Where(x => x.Key.HtmlPage == htmlPage).Select(d => d.Value).All(v => v == DeclarationUsageType.Overriden))
            {
                return true;
            }
            return false;
        }

        public void TryToOverrideBy(HtmlNodeModel htmlNodeModel, Declaration newDeclaration)
        {
            var existingNode = HtmlNodeUsageTypes.Keys.FirstOrDefault(k => k.Equals(htmlNodeModel));
            if (existingNode == null)
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
                    HtmlNodeUsageTypes[existingNode] = DeclarationUsageType.Overriden;
                }
            }
        }

        public void ApplyToHtmlNode(HtmlNodeModel htmlNodeModel)
        {
            var existingNode = HtmlNodeUsageTypes.Keys.FirstOrDefault(k => k.Equals(htmlNodeModel));
            if (existingNode != null)
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