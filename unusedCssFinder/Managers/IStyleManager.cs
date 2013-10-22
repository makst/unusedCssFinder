using System;
using ExCSS;

namespace unusedCssFinder.Managers
{
    public interface IStyleManager
    {
        Stylesheet GetStylesheetFromAddress(string address);
    }
}