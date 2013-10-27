using System.Collections.Generic;
using System.IO;
using System.Linq;
using unusedCssFinder.Models.Statistics;
using unusedCssFinder.Resources;

namespace unusedCssFinder.Utils
{
    public class OutputGenerator
    {
        private const string DESCRIPTION_TEMPLATE = "[DESCRIPTION]";
        private const string UNUSED_ID_TEMPLATE = "[UNUSED_ID]";
        private const string UNUSED_DATA_DESCRIPTION_TEMPLATE = "[UNUSED_DATA_DESCRIPTION]";
        private const string UNUSED_DATA_COUNT_TEMPLATE = "[UNUSED_DATA_COUNT]";
        private const string UNUSED_DATA_TEMPLATE = "[UNUSED_DATA]";

        private int idCounter = 0;

        public void Generate(string fileName, UsageStatisticsModel usageStatisticsModels)
        {
            var data = "";
            data += Resource.Html_top;
            var totalStatisticsModel = GetItemStatistics(usageStatisticsModels.TotalStatisticsModel);
            data += totalStatisticsModel;
            if (usageStatisticsModels.PerPageStatisticsModel.Count > 0)
            {
                var perPageStatisticsHeader = Resource.Description_template.Replace(DESCRIPTION_TEMPLATE,
                                                 "Statistics per page");
                data += perPageStatisticsHeader;
                foreach (var itemStatisticsModel in usageStatisticsModels.PerPageStatisticsModel)
                {
                    var pageStatisticsData = GetItemStatistics(itemStatisticsModel);
                    data += pageStatisticsData;
                }
            }
            data += Resource.Html_bottom;
            WriteToFile(fileName, data);
        }

        private string GetItemStatistics(ItemStatisticsModel model)
        {
            string result = "";
            var statisticsHeader = Resource.Description_template_page.Replace(DESCRIPTION_TEMPLATE,
                                                 model.ItemDescription);
            result += statisticsHeader;
            foreach (var stylesheetStatisticsModel in model.StylesheetStatisticsModels)
            {
                var stylesheetDescription = Resource.Description_template.Replace(DESCRIPTION_TEMPLATE,
                                                 stylesheetStatisticsModel.StylesheetDescription);
                result += stylesheetDescription;

                result += Resource.Stylesheet_statistics_top;
                var unusedSelectorsData = GetUnusedData("Unused selectors", stylesheetStatisticsModel.UnusedSelestors.ToList());
                result += unusedSelectorsData;

                var overridenSelectorsData = GetUnusedData("Always overriden selectors", stylesheetStatisticsModel.OverridenSelectors.ToList());
                result += overridenSelectorsData;

                var overridenDeclarationsData = GetUnusedData("Always overriden declarations", stylesheetStatisticsModel.OverridenDeclarations.ToList());
                result += overridenDeclarationsData;
                result += Resource.Stylesheet_statistics_bottom;

            }
            return result;
        }

        private string GetUnusedData(string description, List<string> inputData)
        {
            var unusedData = Resource.Unused_data_template;
            unusedData = unusedData.Replace(UNUSED_ID_TEMPLATE, idCounter++.ToString("G"));
            unusedData = unusedData.Replace(UNUSED_DATA_DESCRIPTION_TEMPLATE, description);
            unusedData = unusedData.Replace(UNUSED_DATA_COUNT_TEMPLATE, inputData.Count().ToString("G"));
            var unusedSelectors = string.Join("<br />", inputData);
            return unusedData.Replace(UNUSED_DATA_TEMPLATE, unusedSelectors);
        }

        private void WriteToFile(string fileName, string data)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            using (FileStream stream = File.OpenWrite(fileName))
            using (var writer = new StreamWriter(stream))
            {
                writer.Write(data);
            }    
        }
    }
}