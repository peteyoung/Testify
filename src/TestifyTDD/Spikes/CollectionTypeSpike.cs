using System;
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

        [SetUp]
        public void SetUp()
        {
            _collectionTypeMap = new Dictionary<Type, Type>
                                     {
                                         {typeof (IList), typeof (ArrayList)},
                                         {typeof (IList<>), typeof (List<>)}
                                     };
        }

        [Test]
        public void Examine_IList_property_and_create_a_new_list()
        {
            // Arrange
            var propertyInfo = GetPropertyInfo<CollectionsDummy, IList>(c => c.IList);
            var propertyType = propertyInfo.PropertyType;
            Type typeToCreate;
            _collectionTypeMap.TryGetValue(propertyType, out typeToCreate);
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
            Type mappedGenericTypeDef;
            _collectionTypeMap.TryGetValue(genericTypeDef, out mappedGenericTypeDef);
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
}
