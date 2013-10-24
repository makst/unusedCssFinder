using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace unusedCssFinder.Utils
{
    public static class InputFilesParser
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
    }
}