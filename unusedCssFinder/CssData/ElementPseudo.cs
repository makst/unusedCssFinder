using System.Collections.Generic;
using System.Linq;

namespace unusedCssFinder.CssData
{
    public class ElementPseudo : IElementData
    {
        private static List<string> pseudoClasses = new List<string>
            {
                "link",
                "visited",
                "active",
                "hover",
                "focus",
                "first-child",
                "lang",
                "nth-child",
                "nth-last-child",
                "nth-of-type",
                "nth-last-of-type",
                "last-child",
                "first-of-type",
                "last-of-type",
                "only-child",
                "only-of-type",
                "root",
                "empty",
                "target",
                "enabled",
                "disabled",
                "checked Pseudo-class",
                "not"
            };

        private static Specificity _pseudoElementSpecificity = new Specificity(0, 0, 1, 0);
        private static Specificity _pseudoClassSpecificity = new Specificity(0, 0, 0, 1);

        public ElementPseudo(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public Specificity Specificity
        {
            get
            {
                return pseudoClasses.Any(Value.StartsWith) ? _pseudoClassSpecificity : _pseudoElementSpecificity;
            }
        }
    }
}