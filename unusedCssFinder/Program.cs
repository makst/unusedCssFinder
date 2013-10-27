using System;
using CommandLine;
using unusedCssFinder.Managers;
using unusedCssFinder.Models.CommandLine;
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
                var validationResult = options.ValidateAndParseInputFiles();
                if (!validationResult.IsValid)
                {
                    WriteErrorHeaderToConsole(validationResult.Error);
                    Console.WriteLine(validationResult.Message);
                    return;
                }
                Execute(options);
                Console.WriteLine("\nFile " + options.OutputFile + " has been created.");
            }
        }

        private static void Execute(AppOptions options)
        {
            var htmlManager = new HtmlManager();
            var htmlPages = htmlManager.GetHtmlPageModels(options.ParsedUris);

            var styleManager = new StyleManager();
            foreach (var htmlPageModel in htmlPages)
            {
                var htmlPageCssStyles = htmlManager.GetHtmlPageCssUris(htmlPageModel);
                styleManager.GenerateStylesheetModels(htmlPageCssStyles, htmlPageModel.DocumentUri);
            }
            var allRetrievedStyleSheets = styleManager.AllProcessedStylesheetModels;

            AutomapperConfig.Init();
            var htmlToCssMapper = new HtmlToCssMapper();
            var htmlPagesStylesheets = htmlToCssMapper.GetMapResult(htmlPages, allRetrievedStyleSheets);

            var stylesheetApplier = new StylesheetApplier();
            foreach (var htmlPageStylesheetsModel in htmlPagesStylesheets)
            {
                foreach (var stylesheet in htmlPageStylesheetsModel.Stylesheets)
                {
                    var sheetToUse = stylesheet.HasBeenAlreadyAdded
                                                ? stylesheet.AddedBeforeSheet
                                                : stylesheet;

                    if (sheetToUse.CanBeUsedToGetUsageData)
                    {
                        stylesheetApplier.ApplySheetToHtmlPage(sheetToUse.CurrentSheetWithUsageData, htmlPageStylesheetsModel.HtmlPage);
                    }
                }
            }

            var usageStatisticsGenerator = new UsageStatisticsGenerator();
            var usageStatisticsModel = usageStatisticsGenerator.GetUsageStatisticsModel(htmlPagesStylesheets);
            var outputGenerator = new OutputGenerator();
            outputGenerator.Generate(options.OutputFile, usageStatisticsModel);
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
