using System.Collections.Generic;

namespace unusedCssFinder.Models.Statistics
{
    public class UsageStatisticsModel
    {
        public ItemStatisticsModel TotalStatisticsModel { get; set; } 
        public List<ItemStatisticsModel> PerPageStatisticsModel { get; set; } 
    }
}