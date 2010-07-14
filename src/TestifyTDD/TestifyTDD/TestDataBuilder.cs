using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TestifyTDD.Helpers;

namespace TestifyTDD
{
    public class TestDataBuilder<TDOMAIN, TTHIS> : ITestDataBuilder<TDOMAIN, TTHIS>
        where TTHIS : TestDataBuilder<TDOMAIN, TTHIS>, new()
    {
        private Dictionary<PropertyInfo, object> _propertyValues = 
            new Dictionary<PropertyInfo, object>();

        private ICollectionTypeMapper _mapper;

        protected IPropertyHelper<TDOMAIN> _helper;

        // Yes I know this is Poor Man's DI, but I'm not sure if I want to involve
        // a DI container/tool when the TDBs may be used in context with another
        // (or the same) DI container/tool. I also have reservations about
        // embedding a factory.
        public TestDataBuilder() : 
            this(CollectionTypeMapper.CreateDefaultMapper(), 
                new PropertyHelper<TDOMAIN>())
        {
        }

        public TestDataBuilder(
            ICollectionTypeMapper mapper,
            IPropertyHelper<TDOMAIN> propertyHelper)
        {
            PostBuildEvent += OnPostBuild;

            _mapper = mapper;
            _helper = propertyHelper;
        }

        public virtual TDOMAIN Build()
        {
            const bool allowNonPublicConstructors = true;
            
            var domainObj = 
                (TDOMAIN)Activator.CreateInstance(
                             typeof(TDOMAIN), allowNonPublicConstructors);
            
            foreach (var propertyInfo in _propertyValues.Keys)
            {
                var value = _propertyValues[propertyInfo];
                var setter = _helper.GetValueSetter(propertyInfo);

                if (IsTestDataBuilder(value))
                    setter(domainObj, GetValueFromBuilder(value));
                else if (IsTestDataBuilderCollection(value))
                    setter(domainObj, CreateEnumerableFromBuilderList(propertyInfo.PropertyType, value));
                else
                    setter(domainObj, value);
            }

            FireOnPostBuild(domainObj);

            //System.Diagnostics.Debug.WriteLine("Built: " + typeof(TDOMAIN).Name + " hash = " + domainObj.GetHashCode());

            return domainObj;
        }

        /**
         * This event is fired at the very end of the call to Build(). It is useful for maintaining
         * or creating bi directional associations in the domain model that would otherwise
         * cause endless recursive calls to various builders' Build() methods.
         */
        public delegate void PostBuildHandler(object sender, PostBuildEventArgs<TDOMAIN> e);
        public event PostBuildHandler PostBuildEvent;
        public virtual void OnPostBuild(object sender, PostBuildEventArgs<TDOMAIN> eventArgs)
        {
            // Does nothing by default
        }

        protected void FireOnPostBuild(TDOMAIN domainObj)
        {
            if (PostBuildEvent == null) return;

            var args = new PostBuildEventArgs<TDOMAIN>(domainObj);
            PostBuildEvent(this, args);
        }

        // TRETURNS is the return type of property
        // TDOMAIN is the type of the class with property as a member
        // TBUILDER is the type of the builder that builds TDOMAIN

        /**
         * Set a property equal to an actual value. Could be an object or a collection depending
         * on the property's type.
         */
        public TTHIS With<TRETURNS>(Expression<Func<TDOMAIN, TRETURNS>> property, TRETURNS value)
        {
            var propertyInfo = _helper.GetPropertyInfo(property);
            SetPropertyValue(propertyInfo, value);
            return (TTHIS)this;
        }

        /**
         * Set a property equal to the value generated by another builder. Other builder's
         * Build() return type must match type of property.
         */
        public TTHIS With<TRETURNS, TBUILDER>(
            Expression<Func<TDOMAIN, TRETURNS>> property,
            ITestDataBuilder<TRETURNS, TBUILDER> builder)
            where TBUILDER : ITestDataBuilder<TRETURNS, TBUILDER>, new()
        {
            var propertyInfo = _helper.GetPropertyInfo(property);
            SetPropertyValue(propertyInfo, builder);
            return (TTHIS)this;
        }

        // TODO: With for adding a collection of builders to a property

        /**
         * Creates a collection from a params array of values and assigns it to a property. 
         * The type of the values must match the generic parameter of the collection type 
         * specified on the property.
         */
        public TTHIS Withs<TSUBDOMAIN>(
            Expression<Func<TDOMAIN, IEnumerable>> property,
            params TSUBDOMAIN[] values)
        {
            var list = new List<TSUBDOMAIN>();

            foreach (var value in values)
                list.Add(value);

            var propertyInfo = _helper.GetPropertyInfo(property);
            SetPropertyValue(propertyInfo, list);

            return (TTHIS)this;
        }

        /**
         * Creates a collection from a params array of builders and assigns it to a property. 
         * The type of the values must match the generic parameter of the collection type 
         * specified on the property.
         */
        public TTHIS WithBuilders<TRETURNSCOLLECTION, TBUILDER>(
            Expression<Func<TDOMAIN, IEnumerable>> property,
            params ITestDataBuilder<TRETURNSCOLLECTION, TBUILDER>[] builders)
            //where TRETURNS : IList<TSUBDOMAIN>
        {
            // Need to extract the generic parameter from TRETURNSCOLLECTION
            // and use it in place of TRETURNSCOLLECTION below
            var list = new List<ITestDataBuilder<TRETURNSCOLLECTION, TBUILDER>>();

            foreach (var builder in builders)
                list.Add(builder);

            var propertyInfo = _helper.GetPropertyInfo(property);
            SetPropertyValue(propertyInfo, list);

            return (TTHIS)this;
        }

        private object GetValueFromBuilder(object testDataBuilder)
        {
            // NOTE: value should have been vetted by IsTestDataBuilder() first

            var buildMethod = testDataBuilder
                                .GetType()
                                .GetMethod("Build",
                                           BindingFlags.Instance | BindingFlags.Public);

            var value = buildMethod.Invoke(testDataBuilder, null);

            return value;
        }

        private IEnumerable CreateEnumerableFromBuilderList(Type collectionType, object builders)
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
            var collection = collectionConstructor.Invoke(new object[] {});

            // Reflect the Add() method
            var addMethod = collection
                                .GetType()
                                .GetMethod("Add",
                                           BindingFlags.Instance | BindingFlags.Public);
            
            // Iterate builders and populate collection
            foreach (var builder in (IEnumerable)builders)
            {
                var value = GetValueFromBuilder(builder);
                addMethod.Invoke(collection, new[] {value});
            }

            return (IEnumerable)collection;
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

        private bool IsTestDataBuilderCollection(object mayBeACollection)
        {
            var mayBeBuilderCollection = mayBeACollection as IEnumerable;

            if (mayBeBuilderCollection == null)
                return false;

            // string implements IEnumerable too (as do many other framework
            // classes. This is a shortcut out of here since strings are so common.
            var mayBeString = mayBeBuilderCollection as string;
            
            if (mayBeString != null)
                return false;

            // NOTE: this is a good place to start with mixed collections
            //       of classes and builders in a single collection
            var foundNonBuilder = false;
            var foundBuilder = false;

            foreach (var mayBeBuilder in mayBeBuilderCollection)
                if (IsTestDataBuilder(mayBeBuilder))
                    foundBuilder = true;
                else
                    foundNonBuilder = true;

            if (foundBuilder & foundNonBuilder)
                throw new ApplicationException(
                    "Collections of test data can not be a mix of concrete objects and TestDataBuilders");

            return foundBuilder;
        }

        public virtual TTHIS But 
        {
            get
            {
                var clone = new TTHIS();

                foreach (var key in _propertyValues.Keys)
                    clone.SetPropertyValue(key, _propertyValues[key]);

                return clone;
            }
        }

        protected void SetPropertyValue<T>(PropertyInfo key, T value)
        {
            if (_propertyValues.ContainsKey(key))
                _propertyValues[key] = value;
            else
                _propertyValues.Add(key, value);
        }
    }
}