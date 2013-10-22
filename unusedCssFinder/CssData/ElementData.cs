using unusedCssFinder.CssData;
using unusedCssFinder.CssData.ExCssModelsWrappers;

namespace unusedCssFinder.CssData
{
    public class ElementData
    {
        public string Value { get; set; }
        public Specificity Specificity { get; set; }

        public override string ToString()
        {
            return Value;
        }
    }
}
