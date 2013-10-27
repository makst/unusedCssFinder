using System.Collections.Generic;

namespace unusedCssFinder.Models.Statistics
{
    public class StylesheetStatisticsModel
    {
        public string StylesheetDescription { get; set; }
        public IEnumerable<string> UnusedSelestors { get; set; }
        public IEnumerable<string> OverridenSelectors { get; set; }
        public IEnumerable<string> OverridenDeclarations { get; set; }
    }
}