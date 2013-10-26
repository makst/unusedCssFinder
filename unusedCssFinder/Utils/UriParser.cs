using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace unusedCssFinder.Utils
{
    public static class UriParser
    {
        public static bool TryGetAddresses(IList<string> addressStrings, out List<Uri> addresses)
        {
            addresses = null;
            try
            {
                var tempAddresses = new List<Uri>();
                foreach (var address in addressStrings)
                {
                    tempAddresses.Add(new Uri(address));
                }

                var firstUri = tempAddresses.First();
                if (addressStrings.Count() == 1 && firstUri.IsFile && firstUri.AbsolutePath.EndsWith("html"))
                {
                    if (!File.Exists(firstUri.AbsolutePath))
                    {
                        return false;
                    }
                }
                else if (!tempAddresses.All(u => (u.Scheme == "http" || u.Scheme == "https") && u.Host == firstUri.Host))
                {
                    return false;
                }
                addresses = tempAddresses;
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }

        public static bool TryParseLinkAsUri(string linkToParse, Uri parentPageUri, out Uri parsed)
        {
            parsed = null;
            var temp = new Uri(parentPageUri, linkToParse);
            if (temp.Host == parentPageUri.Host)
            {
                if (temp.IsFile && !temp.AbsoluteUri.EndsWith(".html"))
                {
                    return false;
                }
                parsed = temp;
                return true;
            }
            return false;
        }
    }
}