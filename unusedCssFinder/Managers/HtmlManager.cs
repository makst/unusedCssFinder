using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using ExCSS;
using ExCSS.Model;
using HtmlAgilityPack;
using unusedCssFinder.Extensions;
using unusedCssFinder.Models;
using UriParser = unusedCssFinder.Utils.UriParser;

namespace unusedCssFinder.Managers
{
    public class HtmlManager
    {
        private StyleManager _styleManager;

        private LinkedList<Uri> _urisLinkedList = new LinkedList<Uri>();
        private LinkedList<HtmlPageModel> _pageModelsLinkedList = new LinkedList<HtmlPageModel>();
        private Uri _currentUri = null;
        private HtmlPageModel _currentHtmlPageModel = null;

        private List<HtmlPageModel> _pageModelsToReturn = new List<HtmlPageModel>(); 

        public HtmlManager(StyleManager styleManager, List<Uri> initialPageUris)
        {
            foreach (var initialPageUri in initialPageUris)
            {
                _urisLinkedList.AddLast(initialPageUri);
            }
            _styleManager = styleManager;
        }

        public List<HtmlPageModel> GetHtmlPageModels(int scanDepth, int maxNumberOfPages)
        {
            SetNextUriAndHtmlPage();

            var htmlPageModels = new List<HtmlPageModel>();
            foreach (var pageUri in pageUris)
            {
                htmlPageModels.Add(GetHtmlPageModel(pageUri));
            }
            if (scanDepth > 0 || maxNumberOfPages > 0)
            {
                FillHtmlPageModelsWithChildren(htmlPageModels, scanDepth, maxNumberOfPages);
            }
            return htmlPageModels;
        }

        private void SetNextUriAndHtmlPage()
        {
            if (_pageModelsLinkedList.Count < _urisLinkedList.Count)
            {
                _currentHtmlPageModel = _pageModelsLinkedList.Last.Value;
                _currentUri = _urisLinkedList.ToList()[_pageModelsLinkedList.Count];
            }
        }

        private void FillHtmlPageModelsWithChildren(List<HtmlPageModel> htmlPageModels, int scanDepth, int maxNumberOfPages)
        {
            foreach (var htmlPageModel in htmlPageModels)
            {
                List<Uri> childPageUris = GetHtmlPageChildUris(htmlPageModel);
                bool isEnoughPages = IsEnoughPagesParsed(htmlPageModels, scanDepth, maxNumberOfPages);
                foreach (var childPageUri in childPageUris)
                {
                    
                }
            }

            
        }

        private bool IsEnoughPagesParsed(List<HtmlPageModel> htmlPageModels, int scanDepth, int maxNumberOfPages)
        {
            if (htmlPageModels.GetNodesDepth() <= scanDepth || htmlPageModels.GetNumberOfPages() <= maxNumberOfPages)
            {
                return true;
            }
            return f
        }

        private HtmlPageModel GetHtmlPageModel(Uri uri)
        {
            var client = new WebClient();
            var doc = new HtmlDocument();
            doc.Load(client.OpenRead(uri));
            return new HtmlPageModel
            {
                CurrentPage = doc,
                documentUri = uri
            };
        }

        private List<Uri> GetHtmlPageChildUris(HtmlPageModel htmlPageModel)
        {
            List<Uri> uris = new List<Uri>();
            HtmlNodeCollection allLinks = htmlPageModel.CurrentPage.DocumentNode.SelectNodes("//a[@href");
            if (allLinks != null)
            {
                foreach (HtmlNode link in allLinks)
                {
                    string linkHref = link.Attributes.First(a => a.Name == "href").Value;
                    Uri linkUri = null;
                    if (UriParser.TryParseLinkAsUri(linkHref, htmlPageModel.documentUri, out linkUri))
                    {
                        uris.Add(linkUri);
                    }
                }
            }
            return uris;
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
