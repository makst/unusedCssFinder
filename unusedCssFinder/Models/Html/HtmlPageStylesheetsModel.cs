using System.Collections.Generic;
using unusedCssFinder.Models.Style;

namespace unusedCssFinder.Models.Html
{
    public class HtmlPageStylesheetsModel
    {
        public HtmlPageModel HtmlPage { get; set; }
        public IEnumerable<StylesheetModel> Stylesheets { get; set; }
    }
}