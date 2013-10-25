using System.Collections.Generic;
using HtmlAgilityPack;

namespace unusedCssFinder.Models
{
    public class HtmlDocumentModel
    {
        public HtmlDocument CurrentDocument { get; set; }
        public List<HtmlDocument> ChildDocuments { get; set; }
    }
}