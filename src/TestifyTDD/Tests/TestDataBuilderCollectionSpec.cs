using System.Collections.Generic;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using TestifyTDD;
using Tests.TestingDomain;
using Tests.TestDataBuilders;

namespace Tests
{
    [TestFixture]
    public class TestDataBuilderCollectionSpec : TestBase
    {
        private AddressBookBuilder _builder;

        [SetUp]
        public void SetUp()
        {
            _builder = new AddressBookBuilder();
        }

        [Test]
        public void Should_assign_precreated_collection_to_property()
        {
            // Arrange
            var address = new Address();
            var addressList = new List<Address> {address};

            // Act
            _builder.With(ab => ab.Addresses, addressList);
            var addressBook = _builder.Build();

            // Assert
            addressBook.ID.ShouldEqual(123L);
            addressBook.Addresses.ShouldNotBeNull();
            addressBook.Addresses.ShouldBeTheSameAs(addressList);
            addressBook.Addresses[0].ShouldNotBeNull();
            addressBook.Addresses[0].ShouldEqual(address);
        }

        [Test]
        public void Should_assign_params_array_of_domain_instances_to_collection_property()
        {
            // Arrange
            var address1 = new Address();
            var address2 = new Address();
            var address3 = new Address();
            var address4 = new Address();
            var address5 = new Address();

            //var addresses = new[] {address1, address2, address3, address4, address5};

            // Act
            _builder.Withs(ab => ab.Addresses, address1, address2, address3, address4, address5);
            var addressBook = _builder.Build();

            // Assert
            addressBook.ID.ShouldEqual(123L);
            addressBook.Addresses.ShouldNotBeNull();
            addressBook.Addresses.Count.ShouldEqual(5);

            CollectionAssert.AreEquivalent(
                new List<Address> { address1, address2, address3, address4, address5 },
                addressBook.Addresses);
        }
    }

    public class AddressBookBuilder : TestDataBuilder<AddressBook, AddressBookBuilder>
    {
        public AddressBookBuilder()
        {
            With(ab => ab.ID, 123L);

            // This With() is omitted b/c the point of AddressBookBuilder's existence
            // is to test the various ways With() can be used to assign a collection/array
            // to an instance's property
            //With(ab => ab.Addresses, new List<Address>()); 
        }
    }
}