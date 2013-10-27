using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ExCSS;
using ExCSS.Model;
using unusedCssFinder.Extensions;
using unusedCssFinder.Models.Style;

namespace unusedCssFinder.Managers
{
    public class StyleManager
    {
        private static List<StylesheetModel> _allProcessedStylesheets = new List<StylesheetModel>();

        public List<StylesheetModel> AllProcessedStylesheetModels
        {
            get { return _allProcessedStylesheets; }
        }

        public void GenerateStylesheetModels(List<Uri> stylesheetUris, Uri htmlUri)
        {
            var stylesheetModels = new List<StylesheetModel>();
            foreach (var sheetModelUri in stylesheetUris)
            {
                StylesheetModel processedSheet;
                if (_allProcessedStylesheets.WasProcessedBefore(sheetModelUri, out processedSheet))
                {
                    AddProcessedBeforeStylesheet(htmlUri, stylesheetModels, processedSheet);
                }
                else
                {
                    AddStylesheet(stylesheetModels, sheetModelUri, htmlUri, false/*is imported*/, null/*parentSheetUri*/);
                }
            }
            _allProcessedStylesheets.AddRange(stylesheetModels);
        }

        private void AddProcessedBeforeStylesheet(Uri htmlUri, List<StylesheetModel> stylesheetModels, StylesheetModel processedSheet)
        {
            StylesheetModel importedStylesheet;
            if (processedSheet.ImportsStylesheet(_allProcessedStylesheets, out importedStylesheet))
            {
                AddProcessedBeforeStylesheet(htmlUri, stylesheetModels, importedStylesheet);
            }
            stylesheetModels.Add(new StylesheetModel
            {
                HasBeenAlreadyAdded = true,
                AddedBeforeSheet = processedSheet,
                IsImported = processedSheet.IsImported,
                CanBeProcessed = processedSheet.CanBeProcessed,
                HtmlUri = htmlUri
            });
        }

        private void AddStylesheet(List<StylesheetModel> stylesheetModels, Uri sheetUri, Uri htmlUri, bool isImported, Uri parentSheetUri)
        {
            var stylesheet = GetStylesheetFromAddress(sheetUri, htmlUri, isImported, parentSheetUri);
            var importDirectives = stylesheet.CurrentSheetRaw.Directives.Where(d => d.Type == DirectiveType.Import);

            foreach (var importDirective in importDirectives)
            {
                var importDirectiveValue = importDirective.Expression.Terms[0].Value;
                Uri linkUri = null;
                if (Utils.UriParser.TryParseLinkAsUri(importDirectiveValue, sheetUri, out linkUri))
                {
                    AddStylesheet(stylesheetModels, linkUri, htmlUri, true/*isImported*/, sheetUri);
                }
            }
            var stylesheetToAdd = GetStylesheetFromAddress(sheetUri, htmlUri, isImported, parentSheetUri);
            stylesheetModels.Add(stylesheetToAdd);
        }

        public StylesheetModel GetStylesheetFromAddress(Uri uri, Uri htmlUri, bool isImported, Uri parentSheetUri)
        {
            var client = new WebClient();

            bool canBeProcessed = true;
            var stylesheet = new Stylesheet();
            try
            {
                var downLoadedStyle = client.DownloadString(uri);
                var parser = new StylesheetParser();
                stylesheet = parser.Parse(downLoadedStyle);
            }
            catch (Exception)
            {
                canBeProcessed = false;
            }
            return new StylesheetModel
            {
                CurrentSheetRaw = stylesheet,
                DocumentUri = uri,
                HtmlUri = htmlUri,
                IsImported = isImported,
                ParentSheetUri = parentSheetUri,
                CanBeProcessed = canBeProcessed
            };
        }
    }
}