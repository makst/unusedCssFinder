using System;
using System.Collections.Generic;
using ExCSS.Model;

namespace unusedCssFinder.Models
{
    public class CssSelectorUsageModel
    {
        public Selector Selector { get; set; }

        public List<DeclarationUsageModel> DeclarationUsageModel { get; set; }
    }
}