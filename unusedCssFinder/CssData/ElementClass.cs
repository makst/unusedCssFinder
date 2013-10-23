namespace unusedCssFinder.CssData
{
    public class ElementClass : IElementData
    {
        private static Specificity _specificity = new Specificity(0, 0, 1, 0);

        public ElementClass(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public Specificity Specificity { get { return _specificity; } }
    }
}
