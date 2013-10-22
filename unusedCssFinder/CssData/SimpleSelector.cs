using UnusedCssFinder.Utils;

namespace UnusedCssFinder.CssData
{
    public class SimpleSelector : ExCSS.Model.SimpleSelector
    {
        private Specificity _specificity = null;

        public new ElementData ElementName { get; set; }
        public new ElementData ID { get; set; }
        public new ElementData Class { get; set; }
        public new ElementData Pseudo { get; set; }
        public new string CombinatorString { get; set; }
        public new SimpleSelector Child { get; set; }

        public Specificity Specificity
        {
            get
            {
                if (_specificity == null)
                {
                    var sc = new SpecificityCounter();
                    _specificity = sc.GetSpecificityOfSelector(this);
                }
                return _specificity;
            }
        }
    }
}
