using Autofac.Core;
using System.Reflection;

namespace Stock.Common
{
    public class PropertyInjectionSelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.CustomAttributes.Any(a => a.AttributeType == typeof(PropertyInjectionAttribute));
        }
    }
}
