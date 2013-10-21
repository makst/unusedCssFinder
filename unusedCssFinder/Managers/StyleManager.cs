using System;
using System.Net;
using ExCSS;

namespace UnusedCssFinder.Managers
{
    public class StyleManager : IStyleManager
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