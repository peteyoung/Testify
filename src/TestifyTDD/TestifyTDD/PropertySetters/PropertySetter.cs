using System.Reflection;
using TestifyTDD.Helpers;

namespace TestifyTDD.PropertySetters
{
    public abstract class PropertySetter<T> : IPropertySetter<T>
    {
        protected PropertyHelper<T> _helper;

        public abstract void SetValueOnProperty(PropertyInfo propertyInfo, T instance, object value);
    }
}
