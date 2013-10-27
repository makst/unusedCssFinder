using System.Collections.Generic;

namespace unusedCssFinder.Models.Statistics
{
    public class ItemStatisticsModel
    {
        public string ItemDescription { get; set; }
        public List<StylesheetStatisticsModel> StylesheetStatisticsModels { get; set; }
    }
}