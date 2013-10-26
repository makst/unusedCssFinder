using System;
using HtmlAgilityPack;

namespace unusedCssFinder.Models.Html
{
    public class HtmlPageModel
    {
        public Uri DocumentUri { get; set; }
        public HtmlDocument CurrentPage { get; set; }
        
        public bool WasProcessed { get; set; }
        public HtmlPageModel ProcessedPage { get; set; }
    }
}