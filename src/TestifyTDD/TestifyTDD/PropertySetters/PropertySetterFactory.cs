using TestifyTDD.Helpers;

namespace TestifyTDD.PropertySetters
{
    public class PropertySetterFactory
    {
        public IPropertySetter<T> GetPropertySetter<T>(object value)
        {
            // TODO: Inject me!
            var propertyHelper = new PropertyHelper<T>();

            if (IsTestDataBuilder(value))
                return new TestDataBuilderPropertySetter<T>(propertyHelper);
            else
                return new PassthroughPropertySetter<T>(propertyHelper);
        }

        private bool IsTestDataBuilder(object mayBeBuilder)
        {
            if (mayBeBuilder == null)
                return false;

            if (mayBeBuilder.GetType().IsValueType)
                return false;

            var iTestDataBuilderTypeDefinition = typeof(ITestDataBuilder<,>);

            var iTestDataBuilderType = mayBeBuilder
                .GetType()
                .GetInterface(iTestDataBuilderTypeDefinition.Name);

            return (iTestDataBuilderType != null);
        }



    }
}
