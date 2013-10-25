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

                if (tempAddresses.All(t => t.IsFile))
                {
                    foreach (var tempAddress in tempAddresses)
                    {
                        if (!File.Exists(tempAddress.AbsolutePath))
                        {
                            return false;
                        }
                    }
                }
                else if (!tempAddresses.All(u => u.Scheme == "http" || u.Scheme == "https"))
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