namespace unusedCssFinder.CssData
{
    public class ElementId : IElementData
    {
        private static Specificity _specificity = new Specificity(0, 1, 0, 0);

        public ElementId(string value)
        {
            Value = value;
        }

        public string Value { get; private set; }

        public Specificity Specificity { get { return _specificity; } } 
    }
}