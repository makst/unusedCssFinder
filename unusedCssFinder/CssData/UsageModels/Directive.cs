using System.Collections.Generic;
using ExCSS.Model;

namespace unusedCssFinder.CssData.UsageModels
{
    public class Directive
    {
        private ExCSS.Model.Directive _directive;

        public Directive(ExCSS.Model.Directive directive)
        {
            _directive = directive;
        }

        public override string ToString()
        {
            return _directive.ToString();
        }

        public DirectiveType Type { get { return _directive.Type; } }
        public string Name { get { return _directive.Name; } }
        public Expression Expression { get { return _directive.Expression; } }
        public List<Medium> Mediums { get { return _directive.Mediums; } }
        public List<Directive> Directives { get; set; }
        public List<RuleSet> RuleSets { get; set; }
        public List<Declaration> Declarations { get; set; }
    }
}
