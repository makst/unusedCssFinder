using System;
using System.Collections.Generic;
using ExCSS;
using HtmlAgilityPack;

namespace unusedCssFinder.Managers
{
    public interface IHtmlManager
    {
        HtmlDocument GetHtmlDocument(Uri uri);
        Dictionary<string, Stylesheet> GetDocumentStylesheets(Uri baseUri, HtmlDocument htmlDocument);
    }
}