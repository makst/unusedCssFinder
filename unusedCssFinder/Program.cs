using System;
using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;
using UnusedCssFinder.CssData;
using UnusedCssFinder.Managers;
using UnusedCssFinder.Utils;

namespace UnusedCssFinder
{
    class Program
    {
        private static IHtmlManager _htmlManager;
        private static IStyleManager _cssManager;

        static Program()
        {
            var dependencyResolver = new DependencyResolver();
            _htmlManager = dependencyResolver.Resolve<IHtmlManager>();
            _cssManager = dependencyResolver.Resolve<IStyleManager>();
        }

        static void Main(string[] args)
        {
            var baseUri = new Uri("http://i.materialise.com");
            var htmlDocument = _htmlManager.GetHtmlDocument(baseUri);
            var styleUris = _htmlManager.GetDocumentStyleUris(baseUri, htmlDocument);

            var stylesheet = _cssManager.GetStylesheetFromAddress(styleUris.First());


            //List<string> selectors = new List<string>();

            //foreach (var ruleSet in styleSheet.RuleSets)
            //{
            //    foreach (var selector in ruleSet.Selectors)
            //    {
            //        foreach (var simpleSelector in selector.SimpleSelectors)
            //        {
            //            AdjustPseudoIfNeeded(simpleSelector);
            //        }
            //        selectors.Add(string.Join(" ", selector));
            //    }
            //}

            //List<string> unusedSelectors = new List<string>();
            //foreach (var selector in selectors)
            //{
            //    if (!dom.Select(selector).Any())
            //    {
            //        unusedSelectors.Add(selector);
            //    }
            //}
        }

        private static void AdjustPseudoIfNeeded(SimpleSelector simpleSelector)
        {
            List<string> notDependentOnBrowserPseudoClasses = new List<string> { ":first-child", ":last-child", ":first-of-type", ":last-of-type", ":only-child", ":only-of-type", ":nth-child(N)", ":nth-of-type(N)", ":nth-last-child(N)", ":nth-last-of-type(N)", ":enabled", ":disabled", ":empty", ":checked", ":root", ":not(S)" };

            if (simpleSelector.Pseudo != null && !notDependentOnBrowserPseudoClasses.Contains(simpleSelector.Pseudo))
            {
                simpleSelector.Pseudo = null;
            }
            if (simpleSelector.Child != null)
            {
                AdjustPseudoIfNeeded(simpleSelector.Child);
            }
        }
    }
}
