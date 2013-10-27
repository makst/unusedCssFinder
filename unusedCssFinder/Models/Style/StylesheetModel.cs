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

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((StylesheetModel)obj);
        }

        protected bool Equals(StylesheetModel other)
        {
            return Equals(DocumentUri.AbsoluteUri, other.DocumentUri.AbsoluteUri) && HasBeenAlreadyAdded == other.HasBeenAlreadyAdded;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((DocumentUri != null ? DocumentUri.AbsoluteUri.GetHashCode() : 0) * 397) * HasBeenAlreadyAdded.GetHashCode();
            }
        }
    }
}