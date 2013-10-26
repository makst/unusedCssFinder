using System.Collections.Generic;

namespace unusedCssFinder.Models.Statistics
{
    public class StatisticsModel
    {
        public List<string> UnusedStyles { get; set; }
        public List<string> OverridenStyles { get; set; }
        public List<string> OverridenDeclarations { get; set; }
    }
}