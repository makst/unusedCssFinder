using System;
using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;

namespace unusedCssFinder.Models
{
    public class CssSelectorUsageModel
    {
        public Selector Selector { get; set; }

        public List<DeclarationUsageModel> DeclarationUsageModel { get; set; }

        public bool IsNotUsed
        {
            get
            {
                if (DeclarationUsageModel == null)
                {
                    return true;
                }
                if (DeclarationUsageModel.Count == 0)
                {
                    return true;
                }
                if (DeclarationUsageModel.All(d => d.IsNotUsed))
                {
                    return true;
                }
                return false;
            }
        }

        public bool IsOverriden
        {
            get
            {
                if (IsNotUsed)
                {
                    return false;
                }
                if (DeclarationUsageModel.All(d => d.IsOverriden))
                {
                    return true;
                }
                return false;
            }
        }
    }
}