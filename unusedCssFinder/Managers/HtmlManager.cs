using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ExCSS;
using ExCSS.Model;
using HtmlAgilityPack;

namespace unusedCssFinder.Managers
{
    public class HtmlManager : IHtmlManager
    {
        private StyleManager _styleManager;

        public HtmlManager(StyleManager styleManager)
        {
            _styleManager = styleManager;
        }

        public HtmlDocument GetHtmlDocument(Uri uri)
        {
            var client = new WebClient();
            var doc = new HtmlDocument();
            var d = client.DownloadString(uri);
            doc.Load(client.OpenRead(uri));
            return doc;
        }

        public Dictionary<string, Stylesheet> GetDocumentStylesheets(Uri baseUri, HtmlDocument htmlDocument)
        {
            var documentStylesheets = new Dictionary<string, Stylesheet>();
            foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//link[@rel=\"stylesheet\"]"))
            {
                var linkHrefValue = link.Attributes.First(a => a.Name == "href").Value;
                var pathToCss = GetCssUriFromATagHrefAttr(baseUri.Host, linkHrefValue);
                UpdateDictionary(new Uri(pathToCss), documentStylesheets);
            }
            return documentStylesheets;
        }

        private void UpdateDictionary(Uri cssFileUri, Dictionary<string, Stylesheet> documentStylesheets, Uri importedFromUri = null)
        {
            var stylesheet = _styleManager.GetStylesheetFromAddress(cssFileUri.AbsoluteUri);
            var importDirectives = stylesheet.Directives.Where(d => d.Type == DirectiveType.Import);

            foreach (var importDirective in importDirectives)
            {
                var importDirectiveValue = importDirective.Expression.Terms[0].Value;
                var fullPath = GetCssUriFromImportDirectiveValue(cssFileUri, importDirectiveValue);
                UpdateDictionary(new Uri(fullPath), documentStylesheets, cssFileUri);
            }
            var key = importedFromUri == null ? cssFileUri.AbsoluteUri
                                              : String.Format("{0} imported from css : '{1}'", cssFileUri.AbsoluteUri, importedFromUri.AbsoluteUri);
            documentStylesheets.Add(key, stylesheet);
        }

        private string GetCssUriFromATagHrefAttr(string host, string aHrefValue)
        {
            return aHrefValue.StartsWith("/")
                             ? String.Format("http://{0}{1}", host, aHrefValue) : aHrefValue;
        }

        private string GetCssUriFromImportDirectiveValue(Uri baseStyleUri, string importDirectiveValue)
        {
            return importDirectiveValue.StartsWith("/")
                     ? String.Format("http://{0}{1}", baseStyleUri.Host, importDirectiveValue)
                     : baseStyleUri.AbsoluteUri.Replace(baseStyleUri.Segments[baseStyleUri.Segments.Length - 1], importDirectiveValue);
        }
    }
}
