using System;
using System.Reflection;
using TestifyTDD.Helpers;

namespace TestifyTDD.PropertySetters
{
    public class TestDataBuilderPropertySetter<T> : IPropertySetter<T>
    {
        private IPropertyHelper<T> _propertyHelper;

        public TestDataBuilderPropertySetter() : this(new PropertyHelper<T>())
        {
        }

        public TestDataBuilderPropertySetter(IPropertyHelper<T> propertyHelper)
        {
            _propertyHelper = propertyHelper;
        }

        public void SetValueOnProperty(PropertyInfo propertyInfo, T instance, object value)
        {
            ValidateValueIsOfTypeTestDataBuilder(value);
            
            var setter = _propertyHelper.GetValueSetter(propertyInfo);
            var valueFromBuilder = GetValueFromBuilder(value);

            setter(instance, valueFromBuilder);
        }

        private object GetValueFromBuilder(object testDataBuilder)
        {
            var buildMethod = testDataBuilder
                                .GetType()
                                .GetMethod("Build",
                                           BindingFlags.Instance | BindingFlags.Public);

            var value = buildMethod.Invoke(testDataBuilder, null);

            return value;
        }

        private void ValidateValueIsOfTypeTestDataBuilder(object value)
        {
            var builder = value
                .GetType()
                .GetInterface(typeof (ITestDataBuilder<,>).Name);

            if (builder == null)
                throw new ApplicationException(
                    string.Format(
                        "{0} cannot process values of type {1}, only TestDataBuilders.",
                        this.GetType().Name,
                        value.GetType().Name));
        }
    }
}
