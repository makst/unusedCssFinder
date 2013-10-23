using unusedCssFinder.CssData;
using unusedCssFinder.CssData.ExCssModelsWrappers;

namespace unusedCssFinder.CssData
{
    public interface IElementData
    {
        Specificity Specificity { get; }
    }
}
