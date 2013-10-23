using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using ThirdParty.MostThingsWeb;
using unusedCssFinder.CssData.ExCssModelsWrappers;
using unusedCssFinder.Managers;
using unusedCssFinder.Models;
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
            var baseUri = new Uri("http://habrahabr.ru");
            var htmlDocument = _htmlManager.GetHtmlDocument(baseUri);
            var styleIdStylesheets = _htmlManager.GetDocumentStylesheets(baseUri, htmlDocument);

           
            var styleIDExtendedStylesheets = styleIdStylesheets
                    .ToDictionary(s => s.Key, s => Mapper.Map<Stylesheet>(s.Value));

            List<CssSelectorUsageModel> cssSelectorUsageModels = new List<CssSelectorUsageModel>();

            foreach (var styleIDExtendedStylesheet in styleIDExtendedStylesheets)
            {
                var selectorDescriptor = new SelectorDescriptor {CssFilePath = styleIDExtendedStylesheet.Key};
                foreach (var ruleSet in styleIDExtendedStylesheet.Value.RuleSets)
                {
                    foreach (var selector in ruleSet.Selectors)
                    {
                        var xpath = css2xpath.Transform(selector.ToString());
                        var htmlNodes = htmlDocument.DocumentNode.SelectNodes(xpath);

                        var cssSelectorUsageModel = new CssSelectorUsageModel
                            {
                                Selector = selector,
                                SelectorDescriptor = selectorDescriptor
                            };

                        foreach (var declaration in ruleSet.Declarations)
                        {
                            cssSelectorUsageModel.DeclarationUsageModel.Add(new DeclarationUsageModel
                                {
                                    Declaration = declaration
                                });
                        }

                        if (htmlNodes != null)
                        {
                            foreach (var htmlNode in htmlNodes)
                            {
                                if (!cssSelectorUsageModel.MatchedSelectors.Any(
                                        s => String.Equals(s, htmlNode.XPath, StringComparison.InvariantCulture)))
                                {
                                    cssSelectorUsageModel.MatchedSelectors.Add(htmlNode.XPath);
                                }
                            }
                        }

                        cssSelectorUsageModels.Add(cssSelectorUsageModel);
                    }
                }
            }
        }
    }
}
