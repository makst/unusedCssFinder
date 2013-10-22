using System;
using System.Collections.Generic;
using unusedCssFinder.Managers;

namespace unusedCssFinder.Utils
{
    public class DependencyResolver
    {
        Dictionary<Type, object> registeredTypes = new Dictionary<Type, object>
        {
            {typeof(IHtmlManager), new HtmlManager(new StyleManager())},
            {typeof(IStyleManager), new StyleManager()}
        };

        public T Resolve<T>()
        {
            Type typeToResolve = typeof (T);
            if (!registeredTypes.ContainsKey(typeToResolve))
            {
                throw new ArgumentException(typeToResolve + " type is not registered");
            }
            return (T)registeredTypes[typeToResolve];
        }
    }
}