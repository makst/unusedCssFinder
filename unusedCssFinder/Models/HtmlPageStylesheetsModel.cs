using System.Collections.Generic;
using ExCSS;

namespace unusedCssFinder.Models
{
    public class HtmlPageStylesheetsModel
    {
        public HtmlPageModel HtmlPage { get; set; }
        public List<Stylesheet> Stylesheets { get; set; }
    }
}