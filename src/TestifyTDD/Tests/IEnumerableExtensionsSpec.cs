using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using Tests.TestDataBuilders;
using Tests.TestingDomain;
using TestifyTDD.PropertySetters;

namespace Tests
{
    [TestFixture]
    public class IEnumerableExtensionsSpec
    {
        private IEnumerable _builderCollection;
        private IEnumerable _nonBuilderCollection;
        private IEnumerable _mixedCollection;
        
        [SetUp]
        public void SetUp()
        {
            _builderCollection = new ArrayList
                              {
                                  new AddressBuilder(),
                                  new AddressBuilder()
                              };

            _nonBuilderCollection = new ArrayList
                              {
                                  new Address(),
                                  new Address()
                              };

            _mixedCollection = new ArrayList
                              {
                                  new AddressBuilder(),
                                  new Address()
                              };
        }

        [Test]
        public void ContainsBuilders_should_return_true_if_collection_contains_a_builder()
        {
            _mixedCollection.ContainsBuilders().ShouldBeTrue();
        }

        [Test]
        public void ContainsBuilders_should_return_false_if_collection_has_no_builders()
        {
            _nonBuilderCollection.ContainsBuilders().ShouldBeFalse();
        }

        [Test]
        public void ContainsNonBuilders_should_return_true_if_collection_contains_a_builder()
        {
            _mixedCollection.ContainsNonBuilders().ShouldBeTrue();
        }

        [Test]
        public void ContainsNonBuilders_should_return_false_if_collection_has_no_builders()
        {
            _builderCollection.ContainsNonBuilders().ShouldBeFalse();
        }

        [Test]
        public void IsEmpty_should_return_true_if_collection_is_empty()
        {
            new ArrayList().IsEmpty().ShouldBeTrue();
        }

        [Test]
        public void IsEmpty_should_return_false_if_collection_has_items()
        {
            _mixedCollection.IsEmpty().ShouldBeFalse();
        }

        [Test]
        public void GetAddMethod_should_return_MethodInfo_for_Add_method()
        {
            // Arrange
            var list = new ArrayList();

            // Act
            var addMethod = list.GetAddMethod();

            // Assert
            addMethod.ShouldNotBeNull();
            addMethod.Name.ShouldEqual("Add");
        }

        [Test]
        public void InvokeAddMethodWith_should_populate_a_collection_with_an_item()
        {
            // Arrange
            var anObject = new object();
            var list = new ArrayList();

            // Act
            list.InvokeAddMethodWith(anObject);

            // Assert
            list.Count.ShouldEqual(1);
            list[0].ShouldBeTheSameAs(anObject);
        }

        [Test]
        public void InvokeAddMethodWith_should_populate_a_generic_collection_with_an_item()
        {
            // Arrange
            var aString = "I'm a string";
            var genericList = new List<string>();

            // Act
            genericList.InvokeAddMethodWith(aString);

            // Assert
            genericList.Count.ShouldEqual(1);
            genericList[0].ShouldBeTheSameAs(aString);
        }
    }
}
