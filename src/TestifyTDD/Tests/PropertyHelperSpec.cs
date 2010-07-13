using System;
using System.Reflection;
using NBehave.Spec.NUnit;
using NUnit.Framework;
using TestifyTDD.Helpers;
using Tests.TestingDomain;

namespace Tests
{
    [TestFixture]
    public class PropertHelperSpec
    {
        private IPropertyHelper<Address> _helper;
        
        [SetUp]
        public void SetUp()
        {
            _helper = new PropertyHelper<Address>();
        }

        [Test]
        public void Get_property_info_using_lambda()
        {
            // Arrange & Act
            var pi1 = _helper.GetPropertyInfo(c => c.Latitude);

            // Assert
            pi1.ShouldNotBeNull();
            pi1.Name.ShouldEqual("Latitude");
        }

        [Test]
        public void Does_getting_PropertyInfo_return_the_same_instance()
        {
            // Arrange
            var cwmpType = typeof(Address);

            // Act
            var propertyInfo_1 = cwmpType.GetProperty("LotSquareFootage", BindingFlags.Instance | BindingFlags.Public);
            var propertyInfo_2 = _helper.GetPropertyInfo(c => c.LotSquareFootage);

            // Assert
            propertyInfo_1.ShouldBeTheSameAs(propertyInfo_2);
            propertyInfo_1.ShouldEqual(propertyInfo_2);
            propertyInfo_1.GetHashCode().ShouldEqual(propertyInfo_2.GetHashCode());
        }

        [Test]
        public void Can_set_property_value()
        {
            // Arrange
            var longVal = 12345L;

            var propertyInfo = _helper.GetPropertyInfo(c => c.LotSquareFootage);
            var setter = _helper.GetValueSetter(propertyInfo);

            var instance = new Address();

            // Act
            setter(instance, longVal);

            // Assert
            instance.LotSquareFootage.ShouldEqual(longVal);
        }

        [Test]
        public void Can_set_private_setter_property_value()
        {
            // Arrange
            var intVal = 123;

            var propertyInfo = _helper.GetPropertyInfo(c => c.LotNumber);
            var setter = _helper.GetValueSetter(propertyInfo);

            var instance = new Address();

            // Act
            setter(instance, intVal);

            // Assert
            instance.LotNumber.ShouldEqual(intVal);
        }

        [Test]
        public void Cant_set_property_value_with_wrong_argument_type()
        {
            // Arrange
            var stringVal = "I'm a string.";

            var propertyInfo = _helper.GetPropertyInfo(c => c.LotSquareFootage);
            var setter = _helper.GetValueSetter(propertyInfo);

            var instance = new Address();

            // Act & Assert
            Assert.Throws<InvalidCastException>(() => setter(instance, stringVal));
        }

        [Test]
        public void Cant_set_value_on_property_with_no_setter()
        {
            // Arrange
            var propertyInfo = _helper.GetPropertyInfo(c => c.DivisionName);

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _helper.GetValueSetter(propertyInfo));
        }

        [Test]
        public void Cant_call_GetPropertyInfo_with_method_lambda()
        {
            // Arrange, Act, & Assert
            Assert.Throws<ArgumentException>(() => _helper.GetPropertyInfo(c => c.GetNeighboringAddress()));
        }

        [Test]
        public void Cant_call_GetPropertyInfo_with_non_property_lambda()
        {
            // Arrange, Act, & Assert
            Assert.Throws<ArgumentException>(() => _helper.GetPropertyInfo(c => c.Latitude * 10));
        }

        [Test]
        public void PropertyInfo_of_base_type_should_have_reflected_type_downcast_to_TDOMAIN_type()
        {
            // Arrange & Act

            // Address.ID is not declared on ID, but on DomainBase which 
            // Address inherits from. We can't down cast PropertyInfo.DeclaringType,
            // but we can down cast PropertyInfo.ReflectedType
            var propertyInfo = _helper.GetPropertyInfo(c => c.ID);

            // Assert
            propertyInfo.DeclaringType.ShouldEqual(typeof(UnitTestDomainBase)); // base type
            propertyInfo.ReflectedType.ShouldEqual(typeof(Address)); // derived type

            // Before down casting propertyInfo, both DeclaringType and ReflectedType have
            // the type of the base class.
        }
    }
}