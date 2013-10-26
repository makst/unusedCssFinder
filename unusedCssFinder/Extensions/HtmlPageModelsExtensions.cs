using System;
using System.Collections.Generic;
using unusedCssFinder.Models;
using unusedCssFinder.Models.Html;

namespace unusedCssFinder.Extensions
{
    public static class HtmlPageModelsExtensions
    {
        public static bool WasProcessedBefore(this List<HtmlPageModel> pageModels, Uri pageModelUri, out HtmlPageModel processedModel)
        {
            processedModel = null;
            foreach (var pageModel in pageModels)
            {
                if (string.Equals(pageModel.DocumentUri.AbsoluteUri, pageModelUri.AbsoluteUri, StringComparison.InvariantCulture))
                {
                    processedModel = pageModel;
                    return true;
                }
            }
            return false;
        }
    }
}