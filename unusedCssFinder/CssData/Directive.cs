using System.Collections.Generic;
using System.Linq;

namespace UnusedCssFinder.CssData
{
    public class Directive : ExCSS.Model.Directive
    {
        public new List<Declaration> Declarations { get; set; }
        public bool EveryDeclarationIsUsed
        {
            get { return Declarations.All(d => d.IsUsed); }
        }
    }
}
