using unusedCssFinder.CssData;
using unusedCssFinder.Utils;

namespace unusedCssFinder.Managers
{
    public class ElementDataManager
    {
        private SpecificityCounter specificityCounter;

        public ElementDataManager(SpecificityCounter specificityCounter)
        {
            this.specificityCounter = specificityCounter;
        }

        public ElementData GetElementDataBy(string value, ElementDataType elementDataType)
        {
            if (string.IsNullOrEmpty(value))
            {
                return null;
            }

            return new ElementData
            {
                Value = value,
                Specificity = specificityCounter.GetSpecificityBy(value, elementDataType)
            };
        }
    }
}
