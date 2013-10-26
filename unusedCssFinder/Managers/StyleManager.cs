using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ExCSS;
using ExCSS.Model;
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

        public void RetrieveStylesheetModels(List<Uri> stylesheetUris, Uri htmlUri)
        {
            var stylesheetModels = new List<StylesheetModel>();
            foreach (var sheetModelUri in stylesheetUris)
            {
                StylesheetModel processedSheet;
                if (_allProcessedStylesheets.WasProcessedBefore(sheetModelUri, out processedSheet))
                {
                    stylesheetModels.Add(new StylesheetModel
                    {
                        WasProcessed = true,
                        ProcessedSheet = processedSheet,
                        HtmlUri = htmlUri
                    });
                }
                else
                {
                    AddStylesheet(stylesheetModels, sheetModelUri, htmlUri);
                }
            }
            _allProcessedStylesheets.AddRange(stylesheetModels);
        }

        private void AddStylesheet(List<StylesheetModel> stylesheetModels, Uri sheetUri, Uri htmlUri)
        {
            var stylesheet = GetStylesheetFromAddress(sheetUri, htmlUri);
            var importDirectives = stylesheet.CurrentSheet.Directives.Where(d => d.Type == DirectiveType.Import);
            
            foreach (var importDirective in importDirectives)
            {
                var importDirectiveValue = importDirective.Expression.Terms[0].Value;
                Uri linkUri = null;
                if (Utils.UriParser.TryParseLinkAsUri(importDirectiveValue, sheetUri, out linkUri))
                {
                    AddStylesheet(stylesheetModels, linkUri, htmlUri);
                }
            }
            stylesheetModels.Add(GetStylesheetFromAddress(sheetUri, htmlUri));
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