using System.Collections.Generic;

namespace unusedCssFinder.CssData.ExCssModelsWrappers
{
    public class Selector
    {
        private ExCSS.Model.Selector _selector;

        public Selector(ExCSS.Model.Selector selector)
        {
            _selector = selector;
        }

        public List<SimpleSelector> SimpleSelectors { get; set; }

        public Specificity Specificity
        {
            get
            {
                var res = new Specificity(0, 0, 0, 0);
                foreach (var simpleSelector in SimpleSelectors)
                {
                    res += simpleSelector.Specificity;
                }
                return res;
            }
        }

        public override string ToString()
        {
            return _selector.ToString();
        }
    }
}