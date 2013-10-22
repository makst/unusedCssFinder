using System.Collections.Generic;
using AutoMapper;
using UnusedCssFinder.CssData;
using UnusedCssFinder.CssData;
using UnusedCssFinder.Managers;
using UnusedCssFinder.Utils;

namespace UnusedCssFinder
{
    public static class AutomapperConfig
    {
        public static void Init()
        {
            Mapper.CreateMap<ExCSS.Model.Directive, Directive>();
            Mapper.CreateMap<ExCSS.Model.Declaration, Declaration>();
            Mapper.CreateMap<ExCSS.Model.RuleSet, RuleSet>();
            Mapper.CreateMap<ExCSS.Model.Selector, Selector>();
            Mapper.CreateMap<ExCSS.Model.SimpleSelector, SimpleSelector>()
                  .ForMember(dest => dest.Class, opt => opt.MapFrom(src => GetElementData(src.Class, ElementDataType.Class)))
                  .ForMember(dest => dest.ID, opt => opt.MapFrom(src => GetElementData(src.ID, ElementDataType.ID)))
                  .ForMember(dest => dest.ElementName, opt => opt.MapFrom(src => GetElementData(src.ElementName, ElementDataType.ElementName)))
                  .ForMember(dest => dest.Pseudo, opt => opt.MapFrom(src => GetElementData(src.Pseudo, ElementDataType.Pseudo)));

            Mapper.CreateMap<ExCSS.Stylesheet, Stylesheet>()
                  .ForMember(dest => dest.MatchedXpathes, opt => opt.MapFrom(src => new Dictionary<int, string>()));
        }

        private static ElementData GetElementData(string s, ElementDataType elementDataType)
        {
            var edm = new ElementDataManager(new SpecificityCounter());
            return edm.GetElementDataBy(s, elementDataType);
        }
    }
}
