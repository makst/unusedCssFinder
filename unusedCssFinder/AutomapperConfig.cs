using System.Collections.Generic;
using AutoMapper;
using unusedCssFinder.CssData.ExCssModelsWrappers;

namespace unusedCssFinder
{
    public static class AutomapperConfig
    {
        public static void Init()
        {
            Mapper.CreateMap<ExCSS.Model.Directive, Directive>().ConvertUsing(d => new Directive(d)
            {
                Directives = Mapper.Map<List<Directive>>(d.Directives),
                RuleSets = Mapper.Map<List<RuleSet>>(d.RuleSets) 
            });
            Mapper.CreateMap<ExCSS.Model.RuleSet, RuleSet>().ConvertUsing(r => new RuleSet(r)
            {
                Selectors = Mapper.Map<List<Selector>>(r.Selectors)
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
