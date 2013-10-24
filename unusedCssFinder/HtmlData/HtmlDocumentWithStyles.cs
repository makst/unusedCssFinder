using System;
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using ThirdParty.MostThingsWeb;
using unusedCssFinder.CssData.UsageModels;
using unusedCssFinder.Utils;

namespace unusedCssFinder.HtmlData
{
    public class HtmlDocumentWithStyles
    {
        private Dictionary<HtmlNode, List<RuleSet>> _htmlNodesRuleSets = new Dictionary<HtmlNode, List<RuleSet>>();

        public HtmlDocument HtmlDocument { get; set; }

        public Dictionary<HtmlNode, List<RuleSet>> HtmlNodesRuleSets
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

        public void ApplySheet(Stylesheet sheet)
        {
            foreach (var ruleSet in sheet.RuleSets)
            {
                    ApplySelector(ruleSet.Selector);
            }
        }

        private void ApplySelector(Selector selector)
        {
            try
            {
                var selectorToUse = SelectorsFixer.GetFixedSelector(selector);
                var xpath = css2xpath.Transform(selectorToUse);
                var matchedNodes = HtmlDocument.DocumentNode.SelectNodes(xpath);
                if (matchedNodes == null)
                {
                    return;
                }
                foreach (var matchedNode in matchedNodes)
                {
                    AddMapping(matchedNode, selector.RuleSet);
                }
            }
            catch (Exception)
            {
                selector.MatchErrorOccured = true;
            }
        }

        private void AddMapping(HtmlNode htmlNode, RuleSet ruleSet)
        {
            var existingNode = HtmlNodesRuleSets.Keys.FirstOrDefault(k => ReferenceEquals(k, htmlNode));
            if (existingNode == null)
            {
                HtmlNodesRuleSets.Add(htmlNode, new List<RuleSet> { ruleSet });
            }
            {
                ChangeUsageOfExistingRules(htmlNode, ruleSet);
            }
            ApplyDeclarationsToNode(ruleSet.Declarations, htmlNode);
            HtmlNodesRuleSets[htmlNode].Add(ruleSet);
        }

        private void ApplyDeclarationsToNode(List<Declaration> declarations, HtmlNode htmlNode)
        {
            foreach (var declaration in declarations)
            {
                declaration.ApplyToHtmlNode(htmlNode);
            }
        }

        private void ChangeUsageOfExistingRules(HtmlNode htmlNode, RuleSet newRuleSet)
        {
            var htmlNodeRuleSets = HtmlNodesRuleSets[htmlNode];
            foreach (var htmlNodesRuleSet in htmlNodeRuleSets)
            {
                ChangeUsageOfRuleDeclarations(htmlNodesRuleSet, newRuleSet, htmlNode);
            }
        }

        private void ChangeUsageOfRuleDeclarations(RuleSet usedRuleSet, RuleSet newRuleSet, HtmlNode htmlNode)
        {
            var usedRuleSetDeclarations = usedRuleSet.Declarations;
            var newRuleSetDeclarations = newRuleSet.Declarations;
            foreach (var usedDeclaration in usedRuleSetDeclarations)
            {
                foreach (var newDeclaration in newRuleSetDeclarations)
                {
                    usedDeclaration.TryToOverrideBy(htmlNode, newDeclaration);
                }
            }
        }
    }
}