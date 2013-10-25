using System.Net;
using ExCSS;

namespace unusedCssFinder.Managers
{
    public class StyleManager
    {
        public Stylesheet GetStylesheetFromAddress(string address)
        {
            var client = new WebClient();
            var downLoadedStyle = client.DownloadString(address);
            var parser = new StylesheetParser();
            return parser.Parse(downLoadedStyle);
        }
    }
}