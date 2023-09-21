using Autofac.Core;
using System.Reflection;

namespace Stock.Common
{
    public class AutowiredPropertySelector : IPropertySelector
    {
        public bool InjectProperty(PropertyInfo propertyInfo, object instance)
        {
            return propertyInfo.CustomAttributes.Any(a => a.AttributeType == typeof(AutowiredPropertyAttribute));
        }
    }
}
