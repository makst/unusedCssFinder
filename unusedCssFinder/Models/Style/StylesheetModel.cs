using System;
using unusedCssFinder.CssData.UsageModels;

namespace unusedCssFinder.Models.Style
{
    public class StylesheetModel
    {
        public Uri DocumentUri { get; set; }
        public Uri HtmlUri { get; set; }
        public ExCSS.Stylesheet CurrentSheetRaw { get; set; }
        public Stylesheet CurrentSheetWithUsageData { get; set; }

        public bool IsImported { get; set; }
        public Uri ParentSheetUri { get; set; }

        public bool HasBeenAlreadyAdded { get; set; }
        public StylesheetModel AddedBeforeSheet { get; set; }

        public bool CanBeProcessed { get; set; }
        public bool CanBeUsedToGetUsageData
        {
            get
            {
                return !HasBeenAlreadyAdded && CanBeProcessed;
            }
        }
    }
}