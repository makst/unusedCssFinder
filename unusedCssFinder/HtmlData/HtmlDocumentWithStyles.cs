using System.Collections.Generic;
using ExCSS.Model;
using HtmlAgilityPack;

namespace unusedCssFinder.HtmlData
{
    public class HtmlDocumentWithStyles
    {
        private Dictionary<HtmlNode, Selector> _htmlNodesSelectors = new Dictionary<HtmlNode, Selector>();

        public HtmlDocument HtmlDocument { get; set; }

        public Dictionary<HtmlNode, Selector> HtmlNodesSelectors
        {
            get
            {
                return _htmlNodesSelectors;
            }
            set
            {
                _htmlNodesSelectors = value;
            }
        }
    }
}