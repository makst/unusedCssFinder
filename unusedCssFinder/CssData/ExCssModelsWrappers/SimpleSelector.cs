using System.Collections.Generic;
using System.Linq;
using ExCSS.Model;

namespace unusedCssFinder.CssData.ExCssModelsWrappers
{
    public class SimpleSelector
    {
        private ExCSS.Model.SimpleSelector _simpleSelector;
        private Specificity _specificity;
        private ElementName _elementName;
        private ElementId _id;
        private ElementClass _class;
        private ElementAttribute _attribute;
        private ElementPseudo _pseudo;
        private List<IElementData> _elementDatas = new List<IElementData>();

        public SimpleSelector(ExCSS.Model.SimpleSelector simpleSelector)
        {
            _simpleSelector = simpleSelector;
            _elementDatas.Add(this.ElementName);
            _elementDatas.Add(this.ID);
            _elementDatas.Add(this.Class);
            _elementDatas.Add(this.Attribute);
            _elementDatas.Add(this.Pseudo);
        }

        public ElementName ElementName
        {
            get
            {
                if (string.IsNullOrEmpty(_simpleSelector.ElementName))
                {
                    return null;
                }
                if (_elementName == null)
                {
                    _elementName = new ElementName(_simpleSelector.ElementName);
                }
                return _elementName;
            }
        }

        public ElementId ID
        {
            get
            {
                if (string.IsNullOrEmpty(_simpleSelector.ID))
                {
                    return null;
                }
                if (_id == null)
                {
                    _id = new ElementId(_simpleSelector.ID);
                }
                return _id;
            }
        }
        public ElementClass Class
        {
            get
            {
                if (string.IsNullOrEmpty(_simpleSelector.Class))
                {
                    return null;
                }
                if (_class == null)
                {
                    _class = new ElementClass(_simpleSelector.Class);
                }
                return _class;
            }
        }
        public ElementPseudo Pseudo
        {
            get
            {
                if (string.IsNullOrEmpty(_simpleSelector.Pseudo))
                {
                    return null;
                }
                if (_pseudo == null)
                {
                    _pseudo = new ElementPseudo(_simpleSelector.Pseudo);
                }
                return _pseudo;
            }
        }

        public SimpleSelector Child { get; set; }

        public ElementAttribute Attribute
        {
            get
            {
                if (_simpleSelector.Attribute == null)
                {
                    return null;
                }
                if (_attribute == null)
                {
                    _attribute = new ElementAttribute(_simpleSelector.Attribute);
                }
                return _attribute;
            }
        }

        public Function Function
        {
            get
            {
                return _simpleSelector.Function;
            }
        }

        public string CombinatorString
        {
            get
            {
                return _simpleSelector.CombinatorString;
            }
        }

        public Combinator? Combinator
        {
            get
            {
                return _simpleSelector.Combinator;
            }
        }

        public Specificity Specificity
        {
            get
            {
                if (_specificity == null)
                {
                    _specificity = _elementDatas.First(e => e != null).Specificity;
                    if (this.Child != null)
                    {
                        _specificity += Child.Specificity;
                    }
                }
                return _specificity;
            }
        }

        public override string ToString()
        {
            return _simpleSelector.ToString();
        }
    }
}
