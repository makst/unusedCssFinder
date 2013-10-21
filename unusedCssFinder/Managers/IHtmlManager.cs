using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace UnusedCssFinder.Managers
{
    public interface IHtmlManager
    {
        HtmlDocument GetHtmlDocument(Uri uri);
        IEnumerable<string> GetDocumentStyleUris(Uri baseUri, HtmlDocument htmlDocument);
    }
}