using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using unusedCssFinder.CssData.UsageModels;

namespace unusedCssFinder
{
    public static class AutomapperConfig
    {
        public static void Init()
        {
            Mapper.CreateMap<ExCSS.Model.Declaration, Declaration>().ConvertUsing(d => new Declaration(d));
            Mapper.CreateMap<ExCSS.Model.Directive, Directive>().ConvertUsing(d => new Directive(d)
            {
                Directives = Mapper.Map<List<Directive>>(d.Directives),
                RuleSets = Mapper.Map<List<RuleSet>>(d.RuleSets),
                Declarations = Mapper.Map<List<Declaration>>(d.Declarations)
            });
            Mapper.CreateMap<ExCSS.Model.RuleSet, List<RuleSet>>().ConvertUsing(r =>
            {
                var ruleSets = new List<RuleSet>();
                foreach (var selector in r.Selectors)
                {
                    var ruleSet = new RuleSet(r)
                    {
                        Selector = Mapper.Map<Selector>(selector),
                        Declarations = Mapper.Map<List<Declaration>>(r.Declarations)
                    };
                    ruleSet.Selector.RuleSet = ruleSet;

                    foreach (var declaration in ruleSet.Declarations)
                    {
                        declaration.RuleSet = ruleSet;
                    }
                    ruleSets.Add(ruleSet);
                }
                return ruleSets;
            });
            Mapper.CreateMap<ExCSS.Model.Selector, Selector>().ConvertUsing(s => new Selector(s)
            {
                SimpleSelectors = Mapper.Map<List<SimpleSelector>>(s.SimpleSelectors)
            });
            Mapper.CreateMap<ExCSS.Model.SimpleSelector, SimpleSelector>().ConvertUsing(s => new SimpleSelector(s)
            {
                Child = Mapper.Map<SimpleSelector>(s.Child)
            });

            Mapper.CreateMap<ExCSS.Stylesheet, Stylesheet>().ConvertUsing(s =>
            {
                var sheet = new Stylesheet(s)
                {
                    Directives = Mapper.Map<List<Directive>>(s.Directives),
                    RuleSets = new List<RuleSet>()
                };
                foreach (var ruleSet in s.RuleSets)
                {
                    sheet.RuleSets.AddRange(Mapper.Map<List<RuleSet>>(ruleSet));
                }
                return sheet;
            });
        }
    }
}
