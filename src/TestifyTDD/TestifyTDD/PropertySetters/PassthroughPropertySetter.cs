using System.Reflection;
using TestifyTDD.Helpers;

namespace TestifyTDD.PropertySetters
{
    public class PassthroughPropertySetter<T> : IPropertySetter<T>
    {
        private IPropertyHelper<T> _propertyHelper;

        public PassthroughPropertySetter() : this(new PropertyHelper<T>())
        {
        }

        public PassthroughPropertySetter(IPropertyHelper<T> propertyHelper)
        {
            _propertyHelper = propertyHelper;
        }

        public void SetValueOnProperty(PropertyInfo propertyInfo, T instance, object value)
        {
            var setter = _propertyHelper.GetValueSetter(propertyInfo);
            setter(instance, value);
        }
    }
}