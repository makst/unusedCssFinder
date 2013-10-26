using System;
using System.Collections.Generic;
using unusedCssFinder.Models;
using unusedCssFinder.Models.Style;

namespace unusedCssFinder.Extensions
{
    public static class StylesheetModelsExtensions
    {
        public static bool WasProcessedBefore(this List<StylesheetModel> sheetModels, Uri sheetModelUri, out StylesheetModel processedStylesheetModel)
        {
            processedStylesheetModel = null;
            foreach (var stylesheetModel in sheetModels)
            {
                if (string.Equals(stylesheetModel.DocumentUri.AbsoluteUri, sheetModelUri.AbsoluteUri, StringComparison.InvariantCulture))
                {
                    processedStylesheetModel = stylesheetModel;
                    return true;
                }
            }
            return false;
        }
    }
}