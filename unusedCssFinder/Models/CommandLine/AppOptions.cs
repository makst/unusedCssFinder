using System;
using System.Collections.Generic;
using CommandLine;
using CommandLine.Text;

namespace unusedCssFinder.Models.CommandLine
{
    public class AppOptions
    {
        [OptionArray('i', "input", Required = true, HelpText = "Input - valid urls, not separated by comma.")]
        public string[] Input { get; set; }

        public List<Uri> ParsedUris { get; set; }

        [Option('f', "outputFile", Required = true, HelpText = "Location of html file with results of analyzing.")]
        public string OutputFile { get; set; }

        [HelpOption]
        public string GetUsage()
        {
            var defaultHelp = HelpText.AutoBuild(this,
              (HelpText current) => HelpText.DefaultParsingErrorsHandler(this, current));
            return defaultHelp.ToString() + @"
Examples:

Find all unused css selectors always overriden css selectors and directives of http://uawebchallenge.com/. Writes results to c:\results.html:

unusedCssFinder -i http://uawebchallenge.com/ -f c:\results.html

Find all unused css selectors, always overriden css selectors and directives of http://uawebchallenge.com/ and http://uawebchallenge.com/partners. Writes results to c:\results.html:

unusedCssFinder -i  http://uawebchallenge.com/ http://uawebchallenge.com/partners -f c:\results.html

Note:

If c:\results.html file existed before, it will be overwritten.
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