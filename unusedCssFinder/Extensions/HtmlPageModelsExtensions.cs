using System.Collections.Generic;
using System.Linq;
using unusedCssFinder.Models;

namespace unusedCssFinder.Extensions
{
    public static class HtmlPageModelsExtensions
    {
        public static int GetNodesDepth(this List<HtmlPageModel> pageModels)
        {
            int depth = 0;
            foreach (var htmlPageModel in pageModels)
            {
                if (htmlPageModel.ChildPages != null)
                {
                    depth = 1 + htmlPageModel.ChildPages.GetNodesDepth();
                }
            }
            return depth;
        }

        public static int GetNumberOfPages(this List<HtmlPageModel> pageModels)
        {
            return pageModels.Sum(m => m.GetNumberOfPages());
        }
    }
}