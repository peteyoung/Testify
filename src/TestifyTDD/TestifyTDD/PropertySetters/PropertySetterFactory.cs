using System.Collections;
using TestifyTDD.Helpers;

namespace TestifyTDD.PropertySetters
{
    public interface IPropertySetterFactory
    {
        IPropertySetter<T> GetPropertySetter<T>(object value);
    }

    public class PropertySetterFactory : IPropertySetterFactory
    {
        public IPropertySetter<T> GetPropertySetter<T>(object value)
        {
            if (IsTestDataBuilder(value))
                return new TestDataBuilderPropertySetter<T>();

            if (IsTestDataBuilderCollection(value))
                return new TestDataBuilderCollectionPropertySetter<T>();
            
            return new PassthroughPropertySetter<T>();
        }

        private bool IsTestDataBuilder(object mayBeBuilder)
        {
            if (mayBeBuilder == null)
                return false;

            if (mayBeBuilder.GetType().IsValueType)
                return false;

            var builder = mayBeBuilder as ITestDataBuilder;

            return (builder != null);
        }

        // Currently many types in the .NET framework implement IEnumerable.
        // Not sure if this is a problem, or if we need to limit the types
        // that TestDataBuilder considers a collection.
        private bool IsTestDataBuilderCollection(object objectOfUnknownType)
        {
            var mayBeABuilderCollection = objectOfUnknownType as IEnumerable;

            if (mayBeABuilderCollection == null)
                return false;

            // string implements IEnumerable too (as do many other framework
            // classes. This is a shortcut out of here since strings are so common.
            var mayBeString = objectOfUnknownType as string;

            if (mayBeString != null)
                return false;

            foreach (var mayBeBuilder in mayBeABuilderCollection)
                if (IsTestDataBuilder(mayBeBuilder))
                    return true;

            return false;
        }
    }
}
