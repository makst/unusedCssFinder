using System.Collections.Generic;
using System.Linq;
using unusedCssFinder.CssData.ExCssModelsWrappers;

namespace unusedCssFinder.Models
{
    public class CssSelectorUsageModel
    {
        private List<string> _matchedSelectors = new List<string>();

        private List<DeclarationUsageModel> _declarationUsageModel = new List<DeclarationUsageModel>();

        public Selector Selector { get; set; }

        public SelectorDescriptor SelectorDescriptor { get; set; }

        public List<string> MatchedSelectors { get { return _matchedSelectors; } set { _matchedSelectors = value; } }

        public List<DeclarationUsageModel> DeclarationUsageModel { get { return _declarationUsageModel; } set { _declarationUsageModel = value; } }

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