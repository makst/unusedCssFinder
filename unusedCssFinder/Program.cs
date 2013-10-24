using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using unusedCssFinder.CssData.UsageModels;
using unusedCssFinder.HtmlData;
using unusedCssFinder.Managers;
using unusedCssFinder.Utils;

namespace unusedCssFinder
{
    class Program
    {
        private static IHtmlManager _htmlManager;
        private static IStyleManager _cssManager;

        static Program()
        {
            AutomapperConfig.Init();
            var dependencyResolver = new DependencyResolver();
            _htmlManager = dependencyResolver.Resolve<IHtmlManager>();
            _cssManager = dependencyResolver.Resolve<IStyleManager>();
        }

        static void Main(string[] args)
        {
            var baseUri = new Uri("http://trinixy.ru");
            var htmlDocument = _htmlManager.GetHtmlDocument(baseUri);
            var docWithStyles = new HtmlDocumentWithStyles { HtmlDocument = htmlDocument };
            var styleIdStylesheets = _htmlManager.GetDocumentStylesheets(baseUri, htmlDocument);

            var styleIDExtendedStylesheets = styleIdStylesheets
                    .ToDictionary(s => s.Key, s => Mapper.Map<Stylesheet>(s.Value));

            foreach (var sheet in styleIDExtendedStylesheets.Values)
            {
                docWithStyles.ApplySheet(sheet);
            }

            var unusedSelectors = styleIDExtendedStylesheets.Values.SelectMany(x => x.RuleSets)
                                                            .Select(x => x.Selector).Where(x => x.IsNotUsed);
            var alwaysOverridenSelectors = styleIDExtendedStylesheets.Values.SelectMany(x => x.RuleSets)
                                                            .Select(x => x.Selector).Where(x => x.IsOverriden);
            var alwaysOverridenDeclarations = styleIDExtendedStylesheets.Values.SelectMany(x => x.RuleSets)
                                                            .SelectMany(x => x.Declarations).Where(x => x.IsOverriden);
        }
    }
}
