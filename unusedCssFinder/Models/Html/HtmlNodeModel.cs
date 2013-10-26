using HtmlAgilityPack;

namespace unusedCssFinder.Models.Html
{
    public class HtmlNodeModel
    {
        public HtmlNode HtmlNode { get; set; }

        public HtmlPageModel HtmlPage { get; set; }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((HtmlNodeModel) obj);
        }

        protected bool Equals(HtmlNodeModel other)
        {
            return Equals(HtmlNode, other.HtmlNode) && Equals(HtmlPage, other.HtmlPage);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((HtmlNode != null ? HtmlNode.GetHashCode() : 0)*397) ^ (HtmlPage != null ? HtmlPage.GetHashCode() : 0);
            }
        }
    }
}