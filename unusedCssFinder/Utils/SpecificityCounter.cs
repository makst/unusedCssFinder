using System.Collections.Generic;
using System.Linq;
using UnusedCssFinder.CssData;

namespace UnusedCssFinder.Utils
{
    public class SpecificityCounter
    {
        private List<string> pseudoClasses = new List<string>
            {
                "link",
                "visited",
                "active",
                "hover",
                "focus",
                "first-child",
                "lang",
                "nth-child",
                "nth-last-child",
                "nth-of-type",
                "nth-last-of-type",
                "last-child",
                "first-of-type",
                "last-of-type",
                "only-child",
                "only-of-type",
                "root",
                "empty",
                "target",
                "enabled",
                "disabled",
                "checked Pseudo-class",
                "not"
            }; 

        public Specificity GetSpecificityBy(string value, ElementDataType elementDataType, bool isInlineStyle = false)
        {
            if (value == "*")
            {
                return new Specificity(0, 0, 0, 0);
            }

            int a = isInlineStyle ? 1 : 0;
            int b = elementDataType == ElementDataType.ID ? 1 : 0;
            int c = IsElementClassOrPseudoClass(value, elementDataType) ? 1 : 0;
            int d = IsElementNameOrPseudoElement(value, elementDataType) ? 1 : 0;
            return new Specificity(a, b, c, d);
        }

        public Specificity GetSpecificityOfSelector(SimpleSelector simpleSelector)
        {
            var result = new Specificity(0, 0, 0, 0);
            if (simpleSelector.ID != null)
            {
                result += simpleSelector.ID.Specificity;
                return GetResultingSpecificity(result, simpleSelector.Child);
            }
            if (simpleSelector.Class != null)
            {
                result += simpleSelector.Class.Specificity;
                return GetResultingSpecificity(result, simpleSelector.Child);
            }
            if (simpleSelector.ElementName != null)
            {
                result += simpleSelector.ElementName.Specificity;
                return GetResultingSpecificity(result, simpleSelector.Child);
            }
            if (simpleSelector.Pseudo != null)
            {
                result += simpleSelector.Pseudo.Specificity;
                return GetResultingSpecificity(result, simpleSelector.Child);
            }
            return result;
        }

        private Specificity GetResultingSpecificity(Specificity currentSelectorSpecificity, SimpleSelector childSelector)
        {
            if (childSelector == null)
            {
                return currentSelectorSpecificity;
            }
            return childSelector.Specificity + currentSelectorSpecificity;
        }

        private bool IsElementNameOrPseudoElement(string value, ElementDataType elementDataType)
        {
            if (elementDataType == ElementDataType.ElementName)
            {
                return true;
            }
            if (elementDataType == ElementDataType.Pseudo && !pseudoClasses.Any(value.Contains))
            {
                return true;
            }
            return false;
        }

        private bool IsElementClassOrPseudoClass(string value, ElementDataType elementDataType)
        {
            if (elementDataType == ElementDataType.Class)
            {
                return true;
            }
            if (elementDataType == ElementDataType.Pseudo && pseudoClasses.Any(value.Contains))
            {
                return true;
            }
            return false;
        }
    }
}
