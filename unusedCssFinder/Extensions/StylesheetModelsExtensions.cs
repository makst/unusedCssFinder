using System;
using System.Collections.Generic;
using System.Linq;
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

        public static bool ImportsStylesheet(this StylesheetModel sheetModel, List<StylesheetModel> sheetModels, out StylesheetModel importedStylesheetModel)
        {
            importedStylesheetModel = sheetModels.FirstOrDefault(s => s.IsImported
                        && string.Equals(s.ParentSheetUri.AbsoluteUri, 
                        sheetModel.DocumentUri.AbsoluteUri, StringComparison.InvariantCulture));

            if (importedStylesheetModel != null)
            {
                return true;
            }
            return false;
        }
    }
}