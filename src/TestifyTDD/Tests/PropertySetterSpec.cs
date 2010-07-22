using System;
using System.Linq.Expressions;
using System.Reflection;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using Rhino.Mocks;
using TestifyTDD;
using TestifyTDD.Helpers;
using TestifyTDD.PropertySetters;
using Tests.TestDataBuilders;
using Tests.TestingDomain;

namespace Tests
{
    [TestFixture, Ignore]
    public class PropertySetterSpec : TestBase
    {
        protected HouseholdBuilder _householdBuilder = new HouseholdBuilder();
        
        [SetUp]
        public virtual void SetUp()
        {
            
        }

        protected Action<T, object> GetSetter<T>(PropertyInfo propertyInfo)
        {
            var setMethod = propertyInfo.GetSetMethod(true);

            var propertyTypeParameter =
                Expression.Parameter(propertyInfo.ReflectedType, "instance type");

            var propertyArgumentTypeParameter =
                Expression.Parameter(typeof(object), "argument type");

            var setterAction = (Action<T, object>)
                Expression.Lambda(
                    Expression.Call(
                        propertyTypeParameter,
                        setMethod,
                        Expression.Convert(
                            propertyArgumentTypeParameter,
                            propertyInfo.PropertyType)),
                    propertyTypeParameter,
                    propertyArgumentTypeParameter).Compile();

            return setterAction;
        }
    }

    [TestFixture]
    public class PassthroughPropertySetterSpec : PropertySetterSpec
    {
        [Test]
        public void PassthroughPropertySetter_should_set_object_as_value_on_property()
        {
            // Arrange
            var familyName = "Jones";
            var household = new Household();
            var familyNamePropertyInfo = (typeof(Household)).GetProperty("FamilyName");
            var familyNameSetterAction = GetSetter<Household>(familyNamePropertyInfo);

            var propertyHelper = M<IPropertyHelper<Household>>();
            propertyHelper
                .Expect(ph => ph.GetValueSetter(familyNamePropertyInfo))
                .Return(familyNameSetterAction);

            IPropertySetter<Household> setter =
                new PassthroughPropertySetter<Household>(propertyHelper);

            // Act
            setter.SetValueOnProperty(familyNamePropertyInfo, household, familyName);

            // Assert
            propertyHelper.VerifyAllExpectations();
            household.FamilyName.ShouldNotBeNull();
            household.FamilyName.ShouldEqual(familyName);
        }
    }

    [TestFixture]
    public class TestDataBuilderPropertySetterSpec : PropertySetterSpec
    {
        private Address _address;
        private Household _household;
        private PropertyInfo _addressPropertyInfo;
        private ITestDataBuilder<Address, AddressBuilder> _addressBuilder;
        private IPropertySetter<Household> _addressSetter;
        private IPropertyHelper<Household> _propertyHelper;

        [SetUp]
        public override void SetUp()
        {
            _address = new Address();
            _household = new Household();

            _addressPropertyInfo = (typeof(Household)).GetProperty("Address");
            var addressSetterAction = GetSetter<Household>(_addressPropertyInfo);

            _addressBuilder = M<ITestDataBuilder<Address, AddressBuilder>>();

            _propertyHelper = M<IPropertyHelper<Household>>();
            _propertyHelper
                .Expect(ph => ph.GetValueSetter(_addressPropertyInfo))
                .Return(addressSetterAction);

            _addressSetter = new TestDataBuilderPropertySetter<Household>(_propertyHelper);
        }

        [Test]
        public void TestDataBuilderPropertySetter_should_throw_ApplicationException_with_non_builder()
        {
            // Arrange
            _addressBuilder.Expect(ab => ab.Build()).Return(_address).Repeat.Any();

            // Act & Assert
            Assert.Throws<ApplicationException>(
                () =>
                _addressSetter.SetValueOnProperty(_addressPropertyInfo, _household, _address));
        }

        [Test]
        public void TestDataBuilderPropertySetter_should_set_value_on_property()
        {
            // Arrange
            _addressBuilder.Expect(ab => ab.Build()).Return(_address).Repeat.Any();

            // Act
            _addressSetter.SetValueOnProperty(_addressPropertyInfo, _household, _addressBuilder);

            // Assert
            _propertyHelper.VerifyAllExpectations();
            _household.Address.ShouldBeTheSameAs(_address);
        }
    }

//    [TestFixture]
//    public class TestDataBuilderCollectionPropertySetterSpec : PropertySetterSpec
//    {
//        private Address _address;
//        private Household _household;
//        private PropertyInfo _addressPropertyInfo;
//        private ITestDataBuilder<Address, AddressBuilder> _addressBuilder;
//        private IPropertySetter<Household> _addressSetter;
//        private IPropertyHelper<Household> _propertyHelper;
//
//        [SetUp]
//        public override void SetUp()
//        {
//            _address = new Address();
//            _household = new Household();
//
//            _addressPropertyInfo = (typeof(Household)).GetProperty("Address");
//            var addressSetterAction = GetSetter<Household>(_addressPropertyInfo);
//
//            _addressBuilder = M<ITestDataBuilder<Address, AddressBuilder>>();
//
//            _propertyHelper = M<IPropertyHelper<Household>>();
//            _propertyHelper
//                .Expect(ph => ph.GetValueSetter(_addressPropertyInfo))
//                .Return(addressSetterAction);
//
//            _addressSetter = new TestDataBuilderPropertySetter<Household>(_propertyHelper);
//        }
//
//        [Test]
//        public void TestDataBuilderPropertySetter_should_throw_ApplicationException_with_non_builder()
//        {
//            // Arrange
//            _addressBuilder.Expect(ab => ab.Build()).Return(_address).Repeat.Any();
//
//            // Act & Assert
//            Assert.Throws<ApplicationException>(
//                () =>
//                _addressSetter.SetValueOnProperty(_addressPropertyInfo, _household, _address));
//        }
//
//        [Test]
//        public void TestDataBuilderPropertySetter_should_set_value_on_property()
//        {
//            // Arrange
//            _addressBuilder.Expect(ab => ab.Build()).Return(_address).Repeat.Any();
//
//            // Act
//            _addressSetter.SetValueOnProperty(_addressPropertyInfo, _household, _addressBuilder);
//
//            // Assert
//            _propertyHelper.VerifyAllExpectations();
//            _household.Address.ShouldBeTheSameAs(_address);
//        }
//    }
}
