using System.Collections.Generic;

namespace unusedCssFinder.Models.Statistics
{
    public class UsageStatisticsModel
    {
        public bool OnlyTotalStatistics { get; set; }
        public List<StatisticsModel> StatisticsModels { get; set; }
    }
}