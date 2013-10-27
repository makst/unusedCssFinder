using System.Collections.Generic;
using System.Linq;
using unusedCssFinder.Models.Html;
using unusedCssFinder.Models.Statistics;
using unusedCssFinder.Models.Style;

namespace unusedCssFinder.Utils
{
    public class UsageStatisticsGenerator
    {
        public UsageStatisticsModel GetUsageStatisticsModel(List<HtmlPageStylesheetsModel> htmlPagesStylesheets)
        {
            var stylesheets = htmlPagesStylesheets.SelectMany(x => x.Stylesheets).ToList();
            var usageModel = new UsageStatisticsModel
            {
                TotalStatisticsModel = GetTotalItemStatisticsModel(stylesheets)
            };
            if (htmlPagesStylesheets.Select(x => x.HtmlPage).Count() == 1)
            {
                return usageModel;
            }

            usageModel.PerPageStatisticsModel = new List<ItemStatisticsModel>();
            foreach (var htmlPagesStylesheet in htmlPagesStylesheets)
            {
                var pageStatisticsModel = GetPageStatisticsModel(htmlPagesStylesheet);
                usageModel.PerPageStatisticsModel.Add(pageStatisticsModel);                    
            }
            return usageModel;
        }

        private ItemStatisticsModel GetPageStatisticsModel(HtmlPageStylesheetsModel htmlPagesStylesheet)
        {
            var pageStatistics = new ItemStatisticsModel
            {
                ItemDescription = htmlPagesStylesheet.HtmlPage.DocumentUri.AbsolutePath,
                StylesheetStatisticsModels = new List<StylesheetStatisticsModel>()
            };

            foreach (var stylesheetModel in htmlPagesStylesheet.Stylesheets)
            {
                var stylesheetToUse = stylesheetModel.HasBeenAlreadyAdded
                                                     ? stylesheetModel.AddedBeforeSheet
                                                     : stylesheetModel;
                var ruleSets = stylesheetToUse.CurrentSheetWithUsageData.RuleSets.ToList();
                var selectors = ruleSets.Select(x => x.Selector).ToList();

                var stylesheetStatisticsModel = new StylesheetStatisticsModel
                {
                    StylesheetDescription = stylesheetToUse.DocumentUri.AbsoluteUri,
                    UnusedSelestors = selectors.Where(x => x.IsNotUsedOnPage(htmlPagesStylesheet.HtmlPage)).Select(x => x.ToString()),
                    OverridenSelectors = selectors.Where(x => x.IsOverridenOnPage(htmlPagesStylesheet.HtmlPage)).Select(x => x.ToString()),
                    OverridenDeclarations = ruleSets.SelectMany(x => x.Declarations)
                                                    .Where(x => x.IsOverridenOnPage(htmlPagesStylesheet.HtmlPage))
                                                    .Select(x => x.ToString())
                };
                pageStatistics.StylesheetStatisticsModels.Add(stylesheetStatisticsModel);
            }
            return pageStatistics;
        }

        private ItemStatisticsModel GetTotalItemStatisticsModel(IEnumerable<StylesheetModel> stylesheetModels)
        {
            var totalStatisticsModel = new ItemStatisticsModel
            {
                ItemDescription = "Total statistics usage",
                StylesheetStatisticsModels = new List<StylesheetStatisticsModel>()
            };
            foreach (var stylesheetModel in stylesheetModels)
            {
                if (!stylesheetModel.HasBeenAlreadyAdded)
                {
                    var ruleSets = stylesheetModel.CurrentSheetWithUsageData.RuleSets.ToList();
                    var selectors = ruleSets.Select(x => x.Selector).ToList();

                    var stylesheetStatisticsModel = new StylesheetStatisticsModel
                    {
                        StylesheetDescription = stylesheetModel.DocumentUri.AbsoluteUri,
                        UnusedSelestors = selectors.Where(x => x.IsNotUsed).Select(x => x.ToString()),
                        OverridenSelectors = selectors.Where(x => x.IsOverriden).Select(x => x.ToString()),
                        OverridenDeclarations = ruleSets.SelectMany(x => x.Declarations).Where(x => x.IsOverriden).Select(x => x.ToString())
                    };
                    totalStatisticsModel.StylesheetStatisticsModels.Add(stylesheetStatisticsModel);
                }
            }
            return totalStatisticsModel;
        }
    }
}