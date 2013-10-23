using ExCSS.Model;

namespace unusedCssFinder.CssData
{
    public class ElementAttribute : IElementData
    {
        private static Specificity _specificity = new Specificity(0, 0, 1, 0);

        public ElementAttribute(Attribute attribute)
        {
            Attribute = attribute;
        }

        public Attribute Attribute { get; private set; }

        public Specificity Specificity { get { return _specificity; } }
    }
}