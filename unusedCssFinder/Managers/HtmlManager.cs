using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;
using unusedCssFinder.Extensions;
using unusedCssFinder.Models;

namespace unusedCssFinder.Managers
{
    public class HtmlManager
    {
        public List<HtmlPageModel> GetHtmlPageModels(List<Uri> pageUris)
        {
            var htmlPageModels = new List<HtmlPageModel>();
            foreach (var pageUri in pageUris)
            {
                HtmlPageModel processedModel;
                if (htmlPageModels.WasProcessedBefore(pageUri, out processedModel ))
                {
                    htmlPageModels.Add(new HtmlPageModel
                    {
                        DocumentUri = pageUri,
                        ProcessedPage = processedModel
                    });
                }
                htmlPageModels.Add(GetHtmlPageModel(pageUri));
            }
            return htmlPageModels;
        }

        private HtmlPageModel GetHtmlPageModel(Uri uri)
        {
            var client = new WebClient();
            var doc = new HtmlDocument();
            doc.Load(client.OpenRead(uri));
            return new HtmlPageModel
            {
                CurrentPage = doc,
                DocumentUri = uri
            };
        }

        public List<Uri> GetHtmlPageCssUris(HtmlPageModel htmlPageModel)
        {
            List<Uri> uris = new List<Uri>();
            HtmlNodeCollection allLinks = htmlPageModel.CurrentPage.DocumentNode.SelectNodes("//link[@rel=\"stylesheet\"]");
            if (allLinks != null)
            {
                foreach (HtmlNode link in allLinks)
                {
                    string linkHref = link.Attributes.First(a => a.Name == "href").Value;
                    Uri linkUri = null;
                    if (Utils.UriParser.TryParseLinkAsUri(linkHref, htmlPageModel.DocumentUri, out linkUri))
                    {
                        uris.Add(linkUri);
                    }
                }
            }
            return uris;
        }
    }
}
