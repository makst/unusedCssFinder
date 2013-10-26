using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using unusedCssFinder.CssData.UsageModels;
using unusedCssFinder.Models;

namespace unusedCssFinder.Utils
{
    public class HtmlToCssMapper
    {
        public List<HtmlPageStylesheetsModel> GetMapResult(List<HtmlPageModel> htmlPages, List<StylesheetModel> allRetrievedStyleSheets)
        {
            foreach (var styleSheet in allRetrievedStyleSheets)
            {
                if (!styleSheet.HasBeenAlreadyAdded)
                {
                    var sheetWithUsageData = Mapper.Map<ExCSS.Stylesheet, Stylesheet>(styleSheet.CurrentSheetRaw);
                    styleSheet.CurrentSheetWithUsageData = sheetWithUsageData;
                }
            }
            List<HtmlPageStylesheetsModel> htmlPageStylesheetsModels = new List<HtmlPageStylesheetsModel>();
            foreach (var htmlPageModel in htmlPages)
            {
                var htmlPageAbsoluteUri = htmlPageModel.DocumentUri.AbsoluteUri;
                var htmlPageStylesheetsModel = new HtmlPageStylesheetsModel
                {
                    HtmlPage = htmlPageModel,
                    Stylesheets = allRetrievedStyleSheets.Where(x => string.Equals(x.HtmlUri.AbsoluteUri, htmlPageAbsoluteUri, StringComparison.InvariantCulture))
                };
                htmlPageStylesheetsModels.Add(htmlPageStylesheetsModel);
            }
            return htmlPageStylesheetsModels;
        }
    }
}