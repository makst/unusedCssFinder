using ExCSS.Model;
using unusedCssFinder.Managers;
using unusedCssFinder.Utils;

namespace unusedCssFinder.CssData.ExCssModelsWrappers
{
    public class SimpleSelector
    {
        private ExCSS.Model.SimpleSelector _simpleSelector;
        private Specificity _specificity;
        private ElementData _elementName;
        private ElementData _class;
        private ElementData _id;
        private ElementData _pseudo;
        private ElementDataManager _elementDataManager;
        private SpecificityCounter _specificityCounter;

        public SimpleSelector(ExCSS.Model.SimpleSelector simpleSelector)
        {
            _simpleSelector = simpleSelector;
            _specificityCounter = new SpecificityCounter();
            _elementDataManager = new ElementDataManager(_specificityCounter);
        }

        public ElementData ElementName
        {
            get
            {
                if (_elementName == null)
                {
                    _elementName = _elementDataManager.GetElementDataBy(_simpleSelector.ElementName, ElementDataType.ElementName);
                }
                return _elementName;
            }
        }

        public ElementData ID
        {
            get
            {
                if (_id == null)
                {
                    _id = _elementDataManager.GetElementDataBy(_simpleSelector.ID, ElementDataType.ID);
                }
                return _id;
            }
        }
        public ElementData Class
        {
            get
            {
                if (_class == null)
                {
                    _class = _elementDataManager.GetElementDataBy(_simpleSelector.Class, ElementDataType.Class);
                }
                return _class;
            }
        }
        public ElementData Pseudo
        {
            get
            {
                if (_pseudo == null)
                {
                    _pseudo = _elementDataManager.GetElementDataBy(_simpleSelector.Pseudo, ElementDataType.Pseudo);
                }
                return _pseudo;
            }
        }

        public SimpleSelector Child { get; set; }

        public Attribute Attribute
        {
            get
            {
                return _simpleSelector.Attribute;
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
                    _specificity = _specificityCounter.GetSpecificityOfSelector(this);
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
