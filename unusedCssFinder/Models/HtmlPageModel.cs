using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using HtmlAgilityPack;

namespace unusedCssFinder.Models
{
    public class HtmlPageModel
    {
        private List<HtmlPageModel> _childPages = new List<HtmlPageModel>();

        public Uri documentUri { get; set; }
        public HtmlDocument CurrentPage { get; set; }
        public List<HtmlPageModel> ChildPages { get { return _childPages; } set { _childPages = value; } }
        public HtmlPageModel ParentPage { get; set; }
        
        public bool WasProcessed { get; set; }
        public HtmlPageModel ProcessedPage { get; set; }

        public int GetNumberOfPages()
        {
            return _childPages.Sum(p => p.GetNumberOfPages()) + 1;
        }
    }
}