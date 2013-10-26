using System;
using System.Collections.Generic;
using HtmlAgilityPack;

namespace unusedCssFinder.Models
{
    public class HtmlNodeModel
    {
        public override int GetHashCode()
        {
            unchecked
            {
                return ((HtmlNode != null ? HtmlNode.GetHashCode() : 0)*397) ^ (HtmlPage != null ? HtmlPage.GetHashCode() : 0);
            }
        }

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

        //public bool Equals(HtmlNodeModel x, HtmlNodeModel y)
        //{
        //    if (ReferenceEquals(x, y)) return true;

        //    if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
        //        return false;

        //    return string.Equals(x.HtmlNode.XPath, y.HtmlNode.XPath, StringComparison.InvariantCulture)
        //        && string.Equals(x.HtmlPage.DocumentUri.AbsoluteUri, y.HtmlPage.DocumentUri.AbsoluteUri, StringComparison.InvariantCulture);
        //}

        //public int GetHashCode(HtmlNodeModel obj)
        //{
        //    if (ReferenceEquals(obj, null)) return 0;

        //    int hashXpath = obj.HtmlNode.XPath == null ? 0 : obj.HtmlNode.XPath.GetHashCode();

        //    int hashAbsoluteUri = obj.HtmlPage.DocumentUri.AbsoluteUri.GetHashCode();

        //    return hashXpath ^ hashAbsoluteUri;
        //}
    }
}