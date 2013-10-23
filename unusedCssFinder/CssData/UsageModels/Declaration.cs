using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;
using HtmlAgilityPack;

namespace unusedCssFinder.CssData.UsageModels
{
    public class Declaration
    {
        private ExCSS.Model.Declaration _declaration;
        private Dictionary<HtmlNode, DeclarationUsageType> _htmlNodeUsageTypes = new Dictionary<HtmlNode, DeclarationUsageType>();

        public Declaration(ExCSS.Model.Declaration declaration)
        {
            _declaration = declaration;
        }

        public Dictionary<HtmlNode, DeclarationUsageType> HtmlNodeUsageTypes
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

        public RuleSet RuleSet { get; set; }
        public string Name { get { return _declaration.Name; } }
        public bool Important { get { return _declaration.Important; } }
        public Expression Expression { get { return _declaration.Expression; } }

        public override string ToString()
        {
            return _declaration.ToString();
        }

        public void ApplyToHtmlNode(HtmlNode matchedNode)
        {
            HtmlNodeUsageTypes.Add(matchedNode, DeclarationUsageType.Used);
        }
    }
}