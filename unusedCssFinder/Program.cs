﻿using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using CommandLine;
using CommandLine.Text;
using HtmlAgilityPack;
using unusedCssFinder.CssData.UsageModels;
using unusedCssFinder.HtmlData;
using unusedCssFinder.Managers;
using unusedCssFinder.Models;
using unusedCssFinder.Utils;

namespace unusedCssFinder
{
    class Program
    {
        static void Main(string[] args)
        {
            var options = new AppOptions();
            if (Parser.Default.ParseArguments(args, options))
            {
                List<Uri> addresses;
                if (!InputFilesParser.TryGetAddresses(options.Input, out addresses))
                {
                    WriteErrorHeaderToConsole("Input files parsing error!");
                    Console.WriteLine("As an input parameter -i tool excepts only valid urls with one host or existing locally html file.");
                    return;
                }

                var htmlManager = new HtmlManager(new StyleManager());
                List<HtmlDocument> htmlDocuments = new List<HtmlDocument>();
                foreach (var address in addresses)
                {
                    htmlDocuments.Add(htmlManager.GetHtmlDocument(address)); 
                }
            }
            else {
                Console.WriteLine("Input parameters cannot be parsed");
            }

            //AutomapperConfig.Init();

            //var htmlManager = new HtmlManager(new StyleManager());
            //var baseUri = new Uri("http://uawebchallenge.com");
            //var htmlDocument = htmlManager.GetHtmlDocument(baseUri);
            //var docWithStyles = new HtmlDocumentWithStyles { HtmlDocument = htmlDocument };
            //var styleIdStylesheets = htmlManager.GetDocumentStylesheets(baseUri, htmlDocument);

            //var styleIDExtendedStylesheets = styleIdStylesheets
            //        .ToDictionary(s => s.Key, s => Mapper.Map<Stylesheet>(s.Value));

            //foreach (var sheet in styleIDExtendedStylesheets.Values)
            //{
            //    docWithStyles.ApplySheet(sheet);
            //}

            //var ruleSets = styleIDExtendedStylesheets.Values.SelectMany(x => x.RuleSets).ToList();
            //var selectorsInfo = ruleSets.Select(x => x.Selector).ToList();

            //var unusedSelectors = selectorsInfo.Where(x => x.IsNotUsed);
            //var unusedSelectorsCount = unusedSelectors.Count();

            //var alwaysOverridenSelectors = selectorsInfo.Where(x => x.IsOverriden);
            //var alwaysOverridenSelectorsCount = alwaysOverridenSelectors.Count();

            //var alwaysOverridenDeclarations = ruleSets.SelectMany(x => x.Declarations).Where(x => x.IsOverriden);
            //var alwaysOverridenDeclarationsCount = alwaysOverridenDeclarations.Count();
        }

        private static void WriteErrorHeaderToConsole(string errorHeader)
        {
            var oldBackgroundColor = Console.BackgroundColor;
            Console.BackgroundColor = ConsoleColor.Red;
            Console.WriteLine(errorHeader);
            Console.BackgroundColor = oldBackgroundColor;
        }
    }
}
