using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;
using HtmlAgilityPack;

namespace unusedCssFinder.Models
{
    public class DeclarationUsageModel
    {
        public Declaration Declaration { get; set; }
        public Dictionary<HtmlNode, DeclarationUsageType> HtmlNodeUsageTypes { get; set; }

        public bool IsNotUsed
        {
            get
            {
                if (HtmlNodeUsageTypes == null)
                {
                    return true;
                }
                if (HtmlNodeUsageTypes.Keys.Count == 0)
                {
                    return true;
                }
                if (HtmlNodeUsageTypes.Values.All(v => v == DeclarationUsageType.NotUsed))
                {
                    return true;
                }
                return false;
            }
        }
    }
}