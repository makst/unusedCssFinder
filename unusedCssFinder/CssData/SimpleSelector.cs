namespace unusedCssFinder.CssData
{
    public class SimpleSelector : ExCSS.Model.SimpleSelector
    {
        public new ElementData ElementName { get; set; }
        public new ElementData ID { get; set; }
        public new ElementData Class { get; set; }
        public new ElementData Pseudo { get; set; }
    }
}
