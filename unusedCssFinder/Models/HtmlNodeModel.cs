using HtmlAgilityPack;

namespace unusedCssFinder.Models
{
    public class HtmlNodeModel
    {
        public HtmlNode HtmlNode { get; set; }
        public HtmlPageModel HtmlPage { get; set; }

        public override bool Equals(object obj)
        {
            if (obj != null && obj is HtmlNodeModel)
            {
                return Equals((HtmlNodeModel)obj);
            }
            return false;
        }

        public bool Equals(HtmlNodeModel other)
        {
            return HtmlNode == other.HtmlNode && HtmlPage == other.HtmlPage;
        }
    }
}