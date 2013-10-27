using System;
using System.Collections.Generic;
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
                if (!tempAddresses.All(u => (u.Scheme == "http" || u.Scheme == "https") && u.Host == firstUri.Host))
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
            Uri temp;
            try
            {
                temp = new Uri(parentPageUri, linkToParse);
            }
            catch (Exception e)
            {
                return false;
            }
            parsed = temp;
            return true;
        }
    }
}