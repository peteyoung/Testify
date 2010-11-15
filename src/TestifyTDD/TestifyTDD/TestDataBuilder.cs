using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using TestifyTDD.DITool;
using TestifyTDD.Helpers;
using TestifyTDD.PropertySetters;

namespace TestifyTDD
{
    public class TestDataBuilder<TDOMAIN, TTHIS> : ITestDataBuilder<TDOMAIN, TTHIS>
        where TTHIS : TestDataBuilder<TDOMAIN, TTHIS>, new()
    {
        private Dictionary<PropertyInfo, object> _propertyValues = 
            new Dictionary<PropertyInfo, object>();

        protected IPropertyHelper<TDOMAIN> _helper;
        protected IPropertySetterFactory _propertySetterFactory;

        // Yes I know this is Poor Man's DI, but I'm not sure if I want to involve
        // a DI container/tool when the TDBs may be used in context with another
        // (or the same) DI container/tool. I also have reservations about
        // embedding a factory.
        public TestDataBuilder() : this(
            new PropertyHelper<TDOMAIN>(),
            new PropertySetterFactory())
        {
        }

//        public TestDataBuilder()
//            : this(
//                (new DIT()).CreateInstance<IPropertyHelper<TDOMAIN>>(),
//                (new DIT()).CreateInstance<IPropertySetterFactory>())
//        {
//        }


        public TestDataBuilder(
            IPropertyHelper<TDOMAIN> propertyHelper,
            IPropertySetterFactory propertySetterFactory)
        {
            PostBuildEvent += OnPostBuild;

            _helper = propertyHelper;
            _propertySetterFactory = propertySetterFactory;
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
                var propertySetter = _propertySetterFactory.GetPropertySetter<TDOMAIN>(value);
                propertySetter.SetValueOnProperty(propertyInfo, domainObj, value);
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