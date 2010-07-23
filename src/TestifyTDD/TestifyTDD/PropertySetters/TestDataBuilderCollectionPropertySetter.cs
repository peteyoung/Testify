using System;
using System.Collections;
using System.Reflection;
using TestifyTDD.Helpers;

namespace TestifyTDD.PropertySetters
{
    public class TestDataBuilderCollectionPropertySetter<T> : IPropertySetter<T>
    {
        private IPropertyHelper<T> _propertyHelper;
        private ICollectionTypeMapper _mapper;


        public TestDataBuilderCollectionPropertySetter() : 
            this(
                new PropertyHelper<T>(),
                CollectionTypeMapper.CreateDefaultMapper())
        {
        }

        public TestDataBuilderCollectionPropertySetter(
                IPropertyHelper<T> propertyHelper,
                ICollectionTypeMapper mapper)
        {
            _propertyHelper = propertyHelper;
            _mapper = mapper;
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
            var addMethod = GetAddMethodFromCollection(collection);

            // Iterate builders, call Build(), and populate collection with built objects
            foreach (var builder in (IEnumerable)builders)
            {
                var value = ((ITestDataBuilder)builder).CallBuildMethod();
                addMethod.Invoke(collection, new[] { value });
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
            // return using CollectionTypeMapper.
            var instantiableType = _mapper.Resolve(collectionType);

            // Get collection type's parameterless constructor
            var collectionConstructor = instantiableType.GetConstructor(Type.EmptyTypes);

            // Create instance of collection
            var collection = collectionConstructor.Invoke(new object[] { });

            return (IEnumerable)collection;
        }

        private MethodInfo GetAddMethodFromCollection(IEnumerable collection)
        {
            // Reflect the Add() method
            var addMethod = collection
                                .GetType()
                                .GetMethod("Add",
                                           BindingFlags.Instance | BindingFlags.Public);

            return addMethod;
        }
        
        private void ValidateValueIsCollectionOfTestDataBuilders(object value)
        {
            var mayBeABuilderCollection = value as IEnumerable;

            if (mayBeABuilderCollection == null)
                throw new ArgumentException(
                    string.Format("A null value was passed into {0}",
                                    this.GetType().Name));

            // string implements IEnumerable too (as do many other framework
            // classes. This is a shortcut out of here since strings are so common.
            var mayBeString = value as string;

            if (mayBeString != null)
                throw new ArgumentException(
                    string.Format("A value of type string/String was passed into {0}",
                                    this.GetType().Name));

            // NOTE: this is a good place to start with mixed collections
            //       of classes and builders in a single collection
            var foundNonBuilder = false;
            var foundBuilder = false;

            // look for the builder interface on all objects in this collection.
            // flag both cases where we do and do not have a builder
            foreach (var mayBeBuilder in mayBeABuilderCollection)
            {
                var builder = mayBeBuilder
                    .GetType()
                    .GetInterface(typeof (ITestDataBuilder<,>).Name);

                foundBuilder |= builder != null;
                foundNonBuilder |= builder == null;
            }

            if (foundBuilder & foundNonBuilder)
                throw new ApplicationException(
                    "Collections of test data cannot be a mix of concrete objects and TestDataBuilders");

            if (!foundBuilder)
                throw new ApplicationException(
                    "This is not a collection of TestDataBuilders");
        }
    }
}
