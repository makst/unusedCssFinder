using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using HtmlAgilityPack;

namespace UnusedCssFinder.Managers
{
    public class HtmlManager : IHtmlManager
    {
        public HtmlDocument GetHtmlDocument(Uri uri)
        {
            var client = new WebClient();
            var doc = new HtmlDocument();
            doc.Load(client.OpenRead(uri));
            return doc;
        }

        public IEnumerable<string> GetDocumentStyleUris(Uri baseUri, HtmlDocument htmlDocument)
        {
            var styles = new List<string>();
            foreach (HtmlNode link in htmlDocument.DocumentNode.SelectNodes("//link[@type=\"text/css\"]"))
            {
                var styleUri = link.Attributes.First(a => a.Name == "href").Value;
                styleUri = styleUri.StartsWith("/") 
                                   ? String.Format("{0}://{1}{2}", baseUri.Scheme, baseUri.Host, styleUri)
                                   : styleUri;
                styles.Add(styleUri);
            }
            return styles;
        }
    }
}
