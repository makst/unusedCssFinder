using System;
using System.Collections.Generic;
using ThirdParty.MostThingsWeb;
using unusedCssFinder.CssData.UsageModels;
using unusedCssFinder.Models;
using unusedCssFinder.Models.Html;

namespace unusedCssFinder.Utils
{
    public class StylesheetApplier
    {
        private Dictionary<HtmlNodeModel, List<RuleSet>> _htmlNodesRuleSets = new Dictionary<HtmlNodeModel, List<RuleSet>>();

        public Dictionary<HtmlNodeModel, List<RuleSet>> HtmlNodesRuleSets
        {
            get
            {
                return _htmlNodesRuleSets;
            }
            set
            {
                _htmlNodesRuleSets = value;
            }
        }

        public void ApplySheetToHtmlPage(Stylesheet sheet, HtmlPageModel htmlPage)
        {
            foreach (var ruleSet in sheet.RuleSets)
            {
                ApplySelector(htmlPage, ruleSet.Selector);
            }
        }

        private void ApplySelector(HtmlPageModel htmlPage, Selector selector)
        {
            try
            {
                var selectorToUse = SelectorsFixer.GetFixedSelector(selector);
                if (string.IsNullOrEmpty(selectorToUse))
                {
                    return;
                }
                var xpath = css2xpath.Transform(selectorToUse);
                
                var matchedNodes = htmlPage.CurrentPage.DocumentNode.SelectNodes(xpath);
                if (matchedNodes == null)
                {
                    return;
                }
                foreach (var matchedNode in matchedNodes)
                {
                    var matchedNodeModel = new HtmlNodeModel
                    {
                        HtmlNode = matchedNode,
                        HtmlPage = htmlPage
                    };
                    AddMapping(matchedNodeModel, selector.RuleSet);
                }
            }
            catch (Exception)
            {
                selector.MatchErrorOccured = true;
            }
        }

        private void AddMapping(HtmlNodeModel htmlNodeModel, RuleSet ruleSet)
        {
            var nodeExists = HtmlNodesRuleSets.ContainsKey(htmlNodeModel);
            if (!nodeExists)
            {
                HtmlNodesRuleSets.Add(htmlNodeModel, new List<RuleSet> { ruleSet });
            }
            ChangeUsageOfExistingRules(htmlNodeModel, ruleSet);
            ApplyDeclarationsToNode(ruleSet.Declarations, htmlNodeModel);
            if (nodeExists)
            {
                HtmlNodesRuleSets[htmlNodeModel].Add(ruleSet);
            }
        }

        private void ApplyDeclarationsToNode(List<Declaration> declarations, HtmlNodeModel htmlNodeModel)
        {
            foreach (var declaration in declarations)
            {
                declaration.ApplyToHtmlNode(htmlNodeModel);
            }
        }

        private void ChangeUsageOfExistingRules(HtmlNodeModel htmlNodeModel, RuleSet newRuleSet)
        {
            var htmlNodeRuleSets = HtmlNodesRuleSets[htmlNodeModel];
            foreach (var htmlNodesRuleSet in htmlNodeRuleSets)
            {
                ChangeUsageOfRuleDeclarations(htmlNodesRuleSet, newRuleSet, htmlNodeModel);
            }
        }

        private void ChangeUsageOfRuleDeclarations(RuleSet usedRuleSet, RuleSet newRuleSet, HtmlNodeModel htmlNodeModel)
        {
            var usedRuleSetDeclarations = usedRuleSet.Declarations;
            var newRuleSetDeclarations = newRuleSet.Declarations;
            foreach (var usedDeclaration in usedRuleSetDeclarations)
            {
                foreach (var newDeclaration in newRuleSetDeclarations)
                {
                    usedDeclaration.TryToOverrideBy(htmlNodeModel, newDeclaration);
                }
            }
        }
    }
}