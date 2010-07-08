using NUnit.Framework;
using NBehave.Spec.NUnit;
using Rhino.Mocks;
using TestifyTDD;
using Tests.TestDataBuilders;
using Tests.TestingDomain;

namespace Tests
{
    [TestFixture]
    public class TestDataBuilderSpec : TestBase
    {
        // Default AddressBuilder values
        internal const string _defaultStreetAddress1 = "street address line 1";
        internal const string _defaultStreetAddress2 = "street address line 2";
        internal const int _defaultStreetNumber = 1;
        internal const long _defaultLotSquareFootage = 2000L;
        internal const bool _defaultIsInUsa = true;
        internal const decimal _defaultLatitude = 235.37m;
        internal const decimal _defaultLongitude = 15.354m;
        internal const int _defaultLotNumber = 6;
        internal const long _defaultAddressId = 12345L;
        internal const bool _defaultAddressIsNew = true;

        // Default AddressBuilder values
        internal const string _defaultFamilyName = "Smith";
        internal const long _defaultHouseholdId = 234985L;
        internal const bool _defaultHouseholdIsNew = true;

        private AddressBuilder _addressBuilder;
        private HouseholdBuilder _householdBuilder;
        
        [SetUp]
        public void SetUp()
        {
            _addressBuilder = new AddressBuilder();
            _householdBuilder = new HouseholdBuilder();
        }
        
        [Test]
        public void Should_set_all_publicly_getable_properties_with_builders_defaults()
        {
            // Arrange & Act
            var address = _addressBuilder.Build();

            // Assert
            string.IsNullOrEmpty(address.StreetAddress_1).ShouldBeFalse();
            address.StreetAddress_1.ShouldEqual(_defaultStreetAddress1);
            address.StreetAddress_2.ShouldEqual(_defaultStreetAddress2);
            address.StreetNumber.ShouldEqual(_defaultStreetNumber);
            address.LotSquareFootage.ShouldEqual(_defaultLotSquareFootage);
            address.IsInUsa.ShouldEqual(_defaultIsInUsa);
            string.IsNullOrEmpty(address.StreetAddress_2).ShouldBeFalse();
            address.Latitude.ShouldEqual(_defaultLatitude);
            address.Longitude.ShouldEqual(_defaultLongitude);
            address.LotNumber.ShouldEqual(_defaultLotNumber);

            // These two test that properties of super-types can be set
            address.ID.ShouldEqual(_defaultAddressId);
            address.IsNew.ShouldEqual(_defaultAddressIsNew);
        }

        [Test]
        public void Should_be_able_to_override_builder_defaults_using_With()
        {
            // Arrange
            var newStreetAddress = "new street address line 1";
            var newLotSquareFootage = 2435L;
            var newLatitude = 156.987m;
            var newId = 986754L;

            _addressBuilder
                .With(c => c.StreetAddress_1, newStreetAddress)
                .With(c => c.LotSquareFootage, newLotSquareFootage)
                .With(c => c.Latitude, newLatitude)
                .With(c => c.ID, newId);

            // Act
            var address = _addressBuilder.Build();

            // Assert
            // default values
            address.StreetNumber.ShouldEqual(_defaultStreetNumber);
            address.IsInUsa.ShouldEqual(_defaultIsInUsa);
            string.IsNullOrEmpty(address.StreetAddress_2).ShouldBeFalse();
            address.StreetAddress_2.ShouldEqual(_defaultStreetAddress2);
            address.LotNumber.ShouldEqual(_defaultLotNumber);
            address.Longitude.ShouldEqual(_defaultLongitude);
            address.IsNew.ShouldEqual(_defaultAddressIsNew);

            // with'd values
            string.IsNullOrEmpty(address.StreetAddress_1).ShouldBeFalse();
            address.StreetAddress_1.ShouldEqual(newStreetAddress);
            address.LotSquareFootage.ShouldEqual(newLotSquareFootage);
            address.Latitude.ShouldEqual(newLatitude);
            address.ID.ShouldEqual(newId);
        }

        [Test]
        public void Should_get_a_new_instance_of_builder_when_calling_But()
        {
            // Arrange & Act
            var builder2 = _addressBuilder.But;

            // Assert
            builder2.ShouldNotBeTheSameAs(_addressBuilder);
            builder2.GetHashCode().ShouldNotEqual(_addressBuilder.GetHashCode());
            builder2.ShouldBeInstanceOfType(_addressBuilder.GetType());
            builder2.GetType().Name.ShouldEqual(_addressBuilder.GetType().Name);
        }

        [Test]
        public void But_then_With_changes_should_not_affect_original_builder()
        {
            // Arrange
            var newStreetAddress = "new street address line 1";
            var newIsInUsa = false;
            var newId = 87654L;

            // Act
            var addressBuilder2 = 
                _addressBuilder.But
                    .With(c => c.StreetAddress_1, newStreetAddress)
                    .With(c => c.IsInUsa, newIsInUsa)
                    .With(c => c.ID, newId);

            var address1 = _addressBuilder.Build();
            var address2 = addressBuilder2.Build();

            // Assert
            address2.StreetNumber.ShouldEqual(address1.StreetNumber);
            address2.LotSquareFootage.ShouldEqual(address1.LotSquareFootage);
            address2.StreetAddress_2.ShouldEqual(address1.StreetAddress_2);
            address2.Latitude.ShouldEqual(address1.Latitude);
            address2.LotNumber.ShouldEqual(address1.LotNumber);
            address2.IsNew.ShouldEqual(address1.IsNew);

            address2.StreetAddress_1.ShouldNotEqual(address1.StreetAddress_1);
            address2.IsInUsa.ShouldNotEqual(address1.IsInUsa);
            address2.ID.ShouldNotEqual(address1.ID);

            address2.StreetAddress_1.ShouldEqual(newStreetAddress);
            address2.IsInUsa.ShouldEqual(newIsInUsa);
            address2.ID.ShouldEqual(newId);
        }

        [Test, Ignore]
        public void Builders_Build_method_should_be_called_from_containing_Builders_Build_BROKEN()
        {
            // Arrange
            var address = _addressBuilder.Build();

            // BUG: This line throws a TypeLoadException which was supposed to have been fixed in Rhino Mocks
            //      3.6. It was a bug that was actually fixed in Castle.DynamicProxy2
            var mockAddressBuilder = M<ITestDataBuilder<Address, AddressBuilder>>();
            mockAddressBuilder.Expect(b => b.Build()).Return(address);

            _householdBuilder.With(b => b.Address, mockAddressBuilder);

            // Act
            var household = _householdBuilder.Build();

            // Assert
            mockAddressBuilder.VerifyAllExpectations();
            household.Address.ShouldBeTheSameAs(address);
        }

        [Test]
        public void Builders_Build_method_should_be_called_from_containing_Builders_Build()
        {
            // Arrange
            var otherStreetAddress1 = "other street address line 1";
            var otherStreetAddress2 = "other street address line 2";
            var otherStreetNumber = 2653;
            var otherLotSquareFootage = 1234L;
            var otherIsInUsa = false;
            var otherLatitude = 256.128m;
            var otherLongitude = 192.96m;
            var otherLotNumber = 23;
            var otherAddressId = 23456L;
            var otherAddressIsNew = false;

            var otherAddressBuilder = _addressBuilder.But
                .With(c => c.StreetAddress_1, otherStreetAddress1)
                .With(c => c.StreetAddress_2, otherStreetAddress2)
                .With(c => c.StreetNumber, otherStreetNumber)
                .With(c => c.LotSquareFootage, otherLotSquareFootage)
                .With(c => c.IsInUsa, otherIsInUsa)
                .With(c => c.Latitude, otherLatitude)
                .With(c => c.Longitude, otherLongitude)
                .With(c => c.LotNumber, otherLotNumber)
                .With(c => c.ID, otherAddressId)
                .With(c => c.IsNew, otherAddressIsNew);

            _householdBuilder.With(c => c.Address, otherAddressBuilder);

            // Act
            var houseHold = _householdBuilder.Build();

            // Assert
            otherAddressBuilder.ShouldNotBeTheSameAs(_addressBuilder); // just to be sure...

            houseHold.FamilyName.ShouldEqual(_defaultFamilyName);
            houseHold.ID.ShouldEqual(_defaultHouseholdId);
            houseHold.IsNew.ShouldEqual(_defaultHouseholdIsNew);

            string.IsNullOrEmpty(houseHold.Address.StreetAddress_1).ShouldBeFalse();
            houseHold.Address.StreetAddress_1.ShouldEqual(otherStreetAddress1);
            houseHold.Address.StreetAddress_2.ShouldEqual(otherStreetAddress2);
            houseHold.Address.StreetNumber.ShouldEqual(otherStreetNumber);
            houseHold.Address.LotSquareFootage.ShouldEqual(otherLotSquareFootage);
            houseHold.Address.IsInUsa.ShouldEqual(otherIsInUsa);
            string.IsNullOrEmpty(houseHold.Address.StreetAddress_2).ShouldBeFalse();
            houseHold.Address.Latitude.ShouldEqual(otherLatitude);
            houseHold.Address.Longitude.ShouldEqual(otherLongitude);
            houseHold.Address.LotNumber.ShouldEqual(otherLotNumber);
            houseHold.Address.ID.ShouldEqual(otherAddressId);
            houseHold.Address.IsNew.ShouldEqual(otherAddressIsNew);
        }

        [Test]
        public void Build_method_should_cause_PostBuildEvent_to_fire()
        {
            // Arrange
            var eventFiredMock = new BuilderEventFiredMock();

            // Act
            eventFiredMock.Build();

            // Assert
            eventFiredMock.OnPostBuildEventFired.ShouldBeTrue();
        }

        [Test]
        public void Build_method_should_cause_PostBuildEvent_to_populate_eventArgs()
        {
            // Arrange
            var eventFiredMock = new BuilderEventFiredMock();

            // Act
            var dummy = eventFiredMock.Build();

            // Assert
            dummy.ShouldNotBeNull();
            eventFiredMock.PostBuildEventArgs.ShouldNotBeNull();
            eventFiredMock.PostBuildEventArgs.BuiltObject.ShouldNotBeNull();
            dummy.ShouldEqual(eventFiredMock.PostBuildEventArgs.BuiltObject);
        }
    }
}