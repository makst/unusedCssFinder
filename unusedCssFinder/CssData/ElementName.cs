namespace unusedCssFinder.CssData
{
    public class ElementName : IElementData
    {
        private static Specificity _specificity = new Specificity(0, 0, 0, 1);

        public ElementName(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public Specificity Specificity { get { return _specificity; } } 
    }
}