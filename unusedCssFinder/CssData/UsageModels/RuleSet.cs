using System.Collections.Generic;
using System.Linq;

namespace unusedCssFinder.CssData.UsageModels
{
    public class RuleSet
    {
        public List<Declaration> Declarations { get; set; }
        public Selector Selector { get; set; }

        public override string ToString()
        {
            var declarationList = Declarations.Select(d => d.ToString());
            var declarations = string.Join(" ", declarationList);
            return Selector + "{" + declarations + "}";
        }
    }
}