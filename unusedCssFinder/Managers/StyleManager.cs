using System;
using System.Collections.Generic;
using System.Net;
using ExCSS;
using unusedCssFinder.Extensions;
using unusedCssFinder.Models;

namespace unusedCssFinder.Managers
{
    public class StyleManager
    {
        private static List<StylesheetModel> _allProcessedStylesheets = new List<StylesheetModel>();

        public List<StylesheetModel> AllProcessedStylesheetModels
        {
            get { return _allProcessedStylesheets; }
        } 

        public void RetrieveStylesheetModels(List<Uri> pageUris, Uri htmlUri)
        {
            var stylesheetModels = new List<StylesheetModel>();
            foreach (var pageUri in pageUris)
            {
                StylesheetModel processedSheet;
                if (_allProcessedStylesheets.WasProcessedBefore(pageUri, out processedSheet))
                {
                    stylesheetModels.Add(new StylesheetModel
                    {
                        WasProcessed = true,
                        ProcessedSheet = processedSheet,
                        HtmlUri = htmlUri
                    });
                }
                stylesheetModels.Add(GetStylesheetFromAddress(pageUri, htmlUri));
            }
            _allProcessedStylesheets.AddRange(stylesheetModels);
        }

        public StylesheetModel GetStylesheetFromAddress(Uri uri, Uri htmlUri)
        {
            var client = new WebClient();
            var downLoadedStyle = client.DownloadString(uri);
            var parser = new StylesheetParser();
            return new StylesheetModel
            {
                CurrentSheet = parser.Parse(downLoadedStyle),
                DocumentUri = uri,
                HtmlUri = htmlUri
            };
        }
    }
}