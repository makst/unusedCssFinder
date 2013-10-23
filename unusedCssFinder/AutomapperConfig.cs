using System.Collections.Generic;
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
            Mapper.CreateMap<ExCSS.Model.RuleSet, RuleSet>().ConvertUsing((r) =>
            {
                var ruleSet = new RuleSet(r)
                {
                    Selectors = Mapper.Map<List<Selector>>(r.Selectors),
                    Declarations = Mapper.Map<List<Declaration>>(r.Declarations)
                };
                foreach (var selector in ruleSet.Selectors)
                {
                    var s = Mapper.Map<Selector>(selector);
                    s.RuleSet = ruleSet;
                }
                foreach (var declaration in ruleSet.Declarations)
                {
                    var d = Mapper.Map<Declaration>(declaration);
                    d.RuleSet = ruleSet;
                }
                return ruleSet;
            });
            Mapper.CreateMap<ExCSS.Model.Selector, Selector>().ConvertUsing(s => new Selector(s)
            {
                SimpleSelectors = Mapper.Map<List<SimpleSelector>>(s.SimpleSelectors)
            });
            Mapper.CreateMap<ExCSS.Model.SimpleSelector, SimpleSelector>().ConvertUsing(s => new SimpleSelector(s)
            {
                Child = Mapper.Map<SimpleSelector>(s.Child)
            });

            Mapper.CreateMap<ExCSS.Stylesheet, Stylesheet>().ConvertUsing(s => new Stylesheet(s)
            {
                Directives = Mapper.Map<List<Directive>>(s.Directives),
                RuleSets = Mapper.Map<List<RuleSet>>(s.RuleSets)
            });
        }
    }
}
