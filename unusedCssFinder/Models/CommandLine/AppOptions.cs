using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;
using unusedCssFinder.Utils;

namespace unusedCssFinder.Models.CommandLine
{
    public class AppOptions
    {
        [OptionArray('i', "input", Required = true, HelpText = "Input - valid urls or existing locally html file.")]
        public string[] Input { get; set; }

        public List<Uri> ParsedUris { get; set; }
            
        [Option('d', "depth", Required = false, DefaultValue = 0, HelpText = "Page analyzing depth. 0 - only specified pages, 1 - specified page and children and so on.")]
        public int SearchDepth { get; set; }

        [Option('p', "maxPagesCount", Required = false, DefaultValue = 0, HelpText = "Maximum number of pages to analyze. 0 means only specified pages.")]
        public int MaxPagesCount { get; set; }

        [Option('f', "outputFile", Required = true, HelpText = "Location of html file with results of analyzing.")]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var defaultHelp = HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            return defaultHelp.ToString() + @"
Examples:

Find all unused css selectors of http://uawebchallenge.com/ with it's children, but maximum 5 pages will be analyzed. Writes results to c:\results.html:

unusedCssFinder -i http://uawebchallenge.com/ -d 1 -p 5 -f c:\results.html

Find all unused css selectors of only c:\test.html, c:\test2.html pages. Writes results to c:\results.html:

unusedCssFinder -i c:\test.html, c:\test2.html -f c:\results.html

Important note:

First of all tool analyzes specified urls. After that child urls are processed.
";
        }

        public ValidationResult ValidateAndParseInputFiles()
        {
            var result = new ValidationResult {IsValid = true};
            try
            {
                var uri = new Uri(this.OutputFile);
            }
            catch (Exception)
            {
                result.IsValid = false;
                result.Error = "outputFile parsing error!";
                result.Message = "As an input parameter -f tool accepts only a valid path.";
                return result;
            }

            List<Uri> parsedUris;
            if (!Utils.UriParser.TryGetAddresses(this.Input, out parsedUris))
            {
                result.IsValid = false;
                result.Error = "input[files] parsing error!";
                result.Message = "As an input parameter -i tool accepts only valid urls of one site or existing locally html file.";
                return result;
            }
            ParsedUris = parsedUris;
            return result;
        }
    }
}