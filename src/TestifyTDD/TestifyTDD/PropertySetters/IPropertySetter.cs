using System.Reflection;

namespace TestifyTDD.PropertySetters
{
    public interface IPropertySetter<T>
    {
        void SetValueOnProperty(PropertyInfo propertyInfo, T instance, object value);
    }
}
