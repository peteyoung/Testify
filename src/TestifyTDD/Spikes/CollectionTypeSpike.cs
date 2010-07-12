﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using NBehave.Spec.NUnit;

namespace Spikes
{
    [TestFixture]
    public class CollectionTypeSpike
    {
        private Dictionary<Type, Type> _collectionTypeMap;
        private CollectionsDummy _collectionsDummy;

        [SetUp]
        public void SetUp()
        {
            _collectionTypeMap = new Dictionary<Type, Type>
                                     {
                                         {typeof (IList), typeof (ArrayList)},
                                         {typeof (IList<>), typeof (List<>)}
                                     };

            _collectionsDummy = new CollectionsDummy();
        }

        [Test]
        public void Examine_IList_property_and_create_a_new_list()
        {
            // Arrange
            var propertyInfo = GetPropertyInfo<CollectionsDummy, IList>(c => c.IList);
            var propertyType = propertyInfo.PropertyType;
            var typeToCreate = _collectionTypeMap.GetTypeByKey(propertyType);
            var collectionConstructor = typeToCreate.GetConstructor(Type.EmptyTypes);

            // Act
            var newList = collectionConstructor.Invoke(new object[] {});

            // Assert
            newList.ShouldNotBeNull();
            newList.GetType().ShouldEqual(typeof(ArrayList));
        }

        [Test]
        public void Examine_IListGeneric_property_and_create_a_new_Generic_List()
        {
            // Arrange
            var propertyInfo = GetPropertyInfo<CollectionsDummy, IList<string>>(c => c.IListString);
            var propertyType = propertyInfo.PropertyType;
            var genericArguments = propertyType.GetGenericArguments();
            var genericTypeDef = propertyType.GetGenericTypeDefinition();
            var mappedGenericTypeDef = _collectionTypeMap.GetTypeByKey(genericTypeDef);
            var instantiableGenericType = mappedGenericTypeDef.MakeGenericType(genericArguments);
            var collectionConstructor = instantiableGenericType.GetConstructor(Type.EmptyTypes);

            // Act
            var newList = collectionConstructor.Invoke(new object[] { });

            // Assert
            newList.ShouldNotBeNull();
            newList.GetType().ShouldEqual(typeof(List<string>));
        }

        public PropertyInfo GetPropertyInfo<TTYPE, TRETURNS>(
            Expression<Func<TTYPE, TRETURNS>> selector)
        {
            Expression body = selector;

            if (body is LambdaExpression)
                body = ((LambdaExpression)body).Body;

            if (body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(
                    string.Format("selector must be a property. Please specify a property of type {0}", typeof(TTYPE)));

            return (PropertyInfo)((MemberExpression)body).Member;
        }
    }

    public class CollectionsDummy
    {
        public IList IList { get; set; }
        public IList<string> IListString { get; set; }
    }

    public static class MapTypes
    {
        public static Type GetTypeByKey(this Dictionary<Type, Type> dictionary, Type keyType)
        {
            if (! dictionary.ContainsKey(keyType))
                return null;

            return dictionary[keyType];
        }
    }
}
