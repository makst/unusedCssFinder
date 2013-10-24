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
                var res = new List<Uri>();
                foreach (var address in addressStrings)
                {
                    res.Add(new Uri(address));
                }
                if (res.All(u => u.IsFile && u.AbsolutePath.EndsWith("html")))
                {
                    foreach (var address in res)
                    {
                        if (!File.Exists(address.AbsolutePath))
                        {
                            return false;
                        }
                    }
                }
                else if (!res.All(u => (u.Scheme == "http" || u.Scheme == "https") && u.Host == res.First().Host))
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return true;
        }
    }
}