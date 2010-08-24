using System;
using System.Collections;
using System.Reflection;
using TestifyTDD.DITool;
using TestifyTDD.Helpers;

namespace TestifyTDD.PropertySetters
{
    public class TestDataBuilderCollectionPropertySetter<T> : IPropertySetter<T>
    {
        private IPropertyHelper<T> _propertyHelper;

        public TestDataBuilderCollectionPropertySetter() : 
            this(new PropertyHelper<T>())
        {
        }

        public TestDataBuilderCollectionPropertySetter(
                IPropertyHelper<T> propertyHelper)
        {
            _propertyHelper = propertyHelper;
        }

        public void SetValueOnProperty(PropertyInfo propertyInfo, T instance, object value)
        {
            ValidateValueIsCollectionOfTestDataBuilders(value);
            
            var setter = _propertyHelper.GetValueSetter(propertyInfo);
            var enumerable = CreateEnumerableFromBuilderList(propertyInfo.PropertyType, value);

            setter(instance, enumerable);
        }

        private IEnumerable CreateEnumerableFromBuilderList(Type collectionType, object builders)
        {
            var collection = CreateEnumerable(collectionType);

            // Iterate builders, call Build(), and populate collection with built objects
            foreach (ITestDataBuilder builder in (IEnumerable)builders)
            {
                var value = builder.InvokeBuildMethod();
                collection.InvokeAddMethodWith(value);
            }

            return collection;
        }

        private IEnumerable CreateEnumerable(Type collectionType)
        {
            /*
            Since we don't know the exact collection type at runtime, 
            we need to use reflection to create a new instance and to
            populate it by invoking Add().
             
            I dedicate this section to my good friend J.R. "Dynamic Man" Garcia.
            */

            // collectionType may be an interface. Lookup the implementation to 
            // return using TypeMapper.
            var instantiableType = (new DIT()).CreateInstance(collectionType);

            // Get collection type's parameterless constructor
            var collectionConstructor = instantiableType.GetConstructor(Type.EmptyTypes);

            // Create instance of collection
            var collection = collectionConstructor.Invoke(new object[] { });

            return (IEnumerable)collection;
        }
        
        private void ValidateValueIsCollectionOfTestDataBuilders(object value)
        {
            ValidateValueIsNotNull(value);
            ValidateValueIsNotString(value);
            ValidateValueIsIEnumerable(value);

            var collection = value as IEnumerable;

            // NOTE: this is a good place to start with mixed collections
            //       of classes and builders in a single collection
            var foundNonBuilder = collection.ContainsNonBuilders();
            var foundBuilder = collection.ContainsBuilders();

            if (foundBuilder & foundNonBuilder)
                throw new ApplicationException(
                    "Collections of test data cannot be a mix of concrete objects and TestDataBuilders");

            if (!foundBuilder)
                throw new ApplicationException(
                    "This is not a collection of TestDataBuilders");

            if (collection.IsEmpty())
                throw new ApplicationException(
                    "The TestDataBuilder collection was empty.");
        }

        private void ValidateValueIsNotString(object value)
        {
            // string implements IEnumerable too.
            var mayBeString = value as string;

            if (mayBeString != null)
                throw new ArgumentException(
                    string.Format("A value of type string/String was passed into {0}",
                                  GetType().Name));
        }

        private void ValidateValueIsIEnumerable(object value)
        {
            var mayBeEnumerable = value as IEnumerable;

            if (mayBeEnumerable == null)
                throw new ArgumentException(
                    string.Format("{0} is not IEnumerable",
                                  GetType().Name));
        }

        private void ValidateValueIsNotNull(object value)
        {
            if (value == null)
                throw new ArgumentException(
                    string.Format("A null value was passed into {0}",
                                    GetType().Name));
        }
    }
}
