using System;
using ExCSS;

namespace UnusedCssFinder.Managers
{
    public interface IStyleManager
    {
        Stylesheet GetStylesheetFromAddress(string address);
    }
}