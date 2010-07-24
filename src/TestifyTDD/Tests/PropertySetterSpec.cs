using System;
using System.Collections;
using System.Collections.Generic;
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
    public class PropertySetterSpec : TestBase
    {
        protected HouseholdBuilder _householdBuilder = new HouseholdBuilder();
        
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
        public void Should_set_object_as_value_on_property()
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
        public void SetUp()
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
        public void Should_set_value_on_property()
        {
            // Arrange
            _addressBuilder.Expect(ab => ab.Build()).Return(_address).Repeat.Any();

            // Act
            _addressSetter.SetValueOnProperty(_addressPropertyInfo, _household, _addressBuilder);

            // Assert
            _propertyHelper.VerifyAllExpectations();
            _household.Address.ShouldBeTheSameAs(_address);
        }

        [Test]
        public void Should_throw_ApplicationException_with_non_builder()
        {
            // Arrange
            _addressBuilder.Expect(ab => ab.Build()).Return(_address).Repeat.Any();

            // Act & Assert
            Assert.Throws<ApplicationException>(
                () =>
                _addressSetter.SetValueOnProperty(_addressPropertyInfo, _household, _address));
        }
    }

    [TestFixture]
    public class TestDataBuilderCollectionPropertySetterSpec : PropertySetterSpec
    {
        private IList<Address> _neighbors;
        private Household _household;
        private PropertyInfo _neighborsPropertyInfo;
        private ITestDataBuilder<Address, AddressBuilder> _addressBuilder_1;
        private ITestDataBuilder<Address, AddressBuilder> _addressBuilder_2;
        private ITestDataBuilder<Address, AddressBuilder> _addressBuilder_3;
        private IList<ITestDataBuilder<Address, AddressBuilder>> _addressBuilders;
        private IPropertySetter<Household> _neighborsSetter;
        private IPropertyHelper<Household> _propertyHelper;
        private ICollectionTypeMapper _collectionMapper;

        [SetUp]
        public void SetUp()
        {
            _household = new Household();

            _neighborsPropertyInfo = (typeof(Household)).GetProperty("Neighbors");
            var neighborsSetterAction = GetSetter<Household>(_neighborsPropertyInfo);

            _neighbors = new List<Address>
                             {
                                 new Address {StreetNumber = 1},
                                 new Address {StreetNumber = 2},
                                 new Address {StreetNumber = 3}
                             };

            _addressBuilder_1 = M<ITestDataBuilder<Address, AddressBuilder>>();
            _addressBuilder_2 = M<ITestDataBuilder<Address, AddressBuilder>>();
            _addressBuilder_3 = M<ITestDataBuilder<Address, AddressBuilder>>();

            _addressBuilder_1.Expect(ab => ab.Build()).Return(_neighbors[0]).Repeat.Any();
            _addressBuilder_2.Expect(ab => ab.Build()).Return(_neighbors[1]).Repeat.Any();
            _addressBuilder_3.Expect(ab => ab.Build()).Return(_neighbors[2]).Repeat.Any();

            _addressBuilders = new List<ITestDataBuilder<Address, AddressBuilder>>
                                   {
                                       _addressBuilder_1,
                                       _addressBuilder_2,
                                       _addressBuilder_3
                                   };

            _propertyHelper = M<IPropertyHelper<Household>>();
            _propertyHelper
                .Expect(ph => ph.GetValueSetter(_neighborsPropertyInfo))
                .Return(neighborsSetterAction);

            _collectionMapper = M<ICollectionTypeMapper>();
            _collectionMapper
                .Expect(cm => cm.Resolve(typeof (IList<Address>)))
                .Return(typeof (List<Address>));

            _neighborsSetter = 
                new TestDataBuilderCollectionPropertySetter<Household>(
                    _propertyHelper,
                    _collectionMapper);
        }

        [Test]
        public void Should_set_value_on_property()
        {
            // Arrange

            // Act
            _neighborsSetter.SetValueOnProperty(
                _neighborsPropertyInfo, 
                _household, 
                _addressBuilders);

            // Assert
            _addressBuilder_1.VerifyAllExpectations();
            _addressBuilder_2.VerifyAllExpectations();
            _addressBuilder_3.VerifyAllExpectations();
            _propertyHelper.VerifyAllExpectations();
            _collectionMapper.VerifyAllExpectations();

            _household.Neighbors.ShouldNotBeNull();
            CollectionAssert.AreEquivalent(_neighbors, _household.Neighbors);
        }

        [Test]
        public void Should_throw_ArgumentException_with_null_value()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () =>
                    _neighborsSetter.SetValueOnProperty(
                        _neighborsPropertyInfo, 
                        _household, 
                        null));
        }

        [Test]
        public void Should_throw_ArgumentException_with_string_value()
        {
            // Arrange

            // Act & Assert
            Assert.Throws<ArgumentException>(
                () =>
                    _neighborsSetter.SetValueOnProperty(
                        _neighborsPropertyInfo, 
                        _household, 
                        "Did you know string implements IEnumerable?"));
        }

        [Test]
        public void Should_throw_ApplicationException_with_mixed_collection()
        {
            // Arrange
            var naughtyList = new ArrayList
                                  {
                                      _addressBuilder_1,
                                      _addressBuilder_2,
                                      _addressBuilder_3,
                                      new Address()
                                  };

            // Act & Assert
            Assert.Throws<ApplicationException>(
                () =>
                    _neighborsSetter.SetValueOnProperty(
                        _neighborsPropertyInfo, 
                        _household,
                        naughtyList));
        }

        [Test]
        public void Should_throw_ApplicationException_with_nonbuilder_generic_collection()
        {
            // Arrange
            var naughtyList = new List<object>
                                  {
                                      new object(),
                                      new object(),
                                      new object()
                                  };

            // Act & Assert
            Assert.Throws<ApplicationException>(
                () =>
                    _neighborsSetter.SetValueOnProperty(
                        _neighborsPropertyInfo,
                        _household,
                        naughtyList));
        }

        [Test]
        public void Should_throw_ApplicationException_with_nonbuilder_nongeneric_collection()
        {
            // Arrange
            var naughtyList = new ArrayList
                                  {
                                      new object(),
                                      new object(),
                                      new object()
                                  };

            // Act & Assert
            Assert.Throws<ApplicationException>(
                () =>
                    _neighborsSetter.SetValueOnProperty(
                        _neighborsPropertyInfo,
                        _household,
                        naughtyList));
        }

        [Test]
        public void Should_throw_ApplicationException_with_empty_collection()
        {
            // Arrange
            var naughtyList = new ArrayList();

            // Act & Assert
            Assert.Throws<ApplicationException>(
                () =>
                    _neighborsSetter.SetValueOnProperty(
                        _neighborsPropertyInfo,
                        _household,
                        naughtyList));
        }

        [Test]
        public void Should_throw_ApplicationException_with_empty_generic_collection()
        {
            // Arrange
            var naughtyList = new List<Address>();

            // Act & Assert
            Assert.Throws<ApplicationException>(
                () =>
                    _neighborsSetter.SetValueOnProperty(
                        _neighborsPropertyInfo,
                        _household,
                        naughtyList));
        }
    }
}
