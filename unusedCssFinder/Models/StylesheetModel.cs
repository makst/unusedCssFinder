using System;

namespace unusedCssFinder.Models
{
    public class StylesheetModel
    {
        public Uri DocumentUri { get; set; }
        public Uri HtmlUri { get; set; }
        public ExCSS.Stylesheet CurrentSheet { get; set; }

        public bool IsImported { get; set; }
        public StylesheetModel ParentSheet { get; set; }
        public bool WasProcessed { get; set; }
        public StylesheetModel ProcessedSheet { get; set; } 
    }
}