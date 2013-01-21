//using System.Collections;
//using System.Collections.Generic;
//using NUnit.Framework;
//using NBehave.Spec.NUnit;
//using TestifyTDD;
//using Tests.TestDataBuilders;
//using Tests.TestingDomain;
//
//namespace Tests
//{
//    [TestFixture]
//    public class TestDataBuilderCollectionSpec : TestBase
//    {
//        private OldAddressBookBuilder _builder;
//
//        [SetUp]
//        public void SetUp()
//        {
//            _builder = new OldAddressBookBuilder();
//        }
//
//        // TODO: the With() method is too strongly typed to IList<T>
////        [Test]
////        public void Should_assign_precreated_collection_to_property()
////        {
////            // Arrange
////            var address = new Address();
////            var addressList = new ArrayList {address};
////
////            // Act
////            _builder.With(ab => ab.Addresses, addressList);
////            var addressBook = _builder.Build();
////
////            // Assert
////            addressBook.ID.ShouldEqual(123L);
////            addressBook.Addresses.ShouldNotBeNull();
////            addressBook.Addresses.ShouldBeTheSameAs(addressList);
////            addressBook.Addresses[0].ShouldNotBeNull();
////            addressBook.Addresses[0].ShouldEqual(address);
////        }
//
//        [Test]
//        public void A_mixed_collection_of_domain_objects_and_builders_should_throw_an_exception()
//        {
//            // Arrange
//            var address1 = new Address();
//            var address2 = new Address();
//            var address3 = new Address();
//            var address4 = new Address();
//            var address5 = new Address();
//
//            //var addresses = new[] {address1, address2, address3, address4, address5};
//
//            // Act
//            _builder.Withs(ob => ob.Addresses, address1, address2, address3, address4, address5);
//            var oldAddressBook = _builder.Build();
//
//            // Assert
//            oldAddressBook.ID.ShouldEqual(123L);
//            oldAddressBook.Addresses.ShouldNotBeNull();
//            oldAddressBook.Addresses.Count.ShouldEqual(5);
//
//            CollectionAssert.AreEquivalent(
//                new ArrayList { address1, address2, address3, address4, address5 },
//                oldAddressBook.Addresses);
//        }
//
//        [Test]
//        public void Should_assign_params_array_of_domain_instances_to_collection_property()
//        {
//            // Arrange
//            var address1 = new Address();
//            var address2 = new Address();
//            var address3 = new Address();
//            var address4 = new Address();
//            var address5 = new Address();
//
//            //var addresses = new[] {address1, address2, address3, address4, address5};
//
//            // Act
//            _builder.Withs(ob => ob.Addresses, address1, address2, address3, address4, address5);
//            var oldAddressBook = _builder.Build();
//
//            // Assert
//            oldAddressBook.ID.ShouldEqual(123L);
//            oldAddressBook.Addresses.ShouldNotBeNull();
//            oldAddressBook.Addresses.Count.ShouldEqual(5);
//
//            CollectionAssert.AreEquivalent(
//                new ArrayList { address1, address2, address3, address4, address5 },
//                oldAddressBook.Addresses);
//        }
//
//        [Test]
//        public void Should_assign_params_array_of_builders_to_collection_property_and_build_them()
//        {
//            // Arrange
//            var builder1 = new AddressBuilder();
//            var builder2 = new AddressBuilder();
//            var builder3 = new AddressBuilder();
//            var builder4 = new AddressBuilder();
//            var builder5 = new AddressBuilder();
//
//            var addressList = new ArrayList
//                                  {
//                                      builder1.Build(),
//                                      builder2.Build(),
//                                      builder3.Build(),
//                                      builder4.Build(),
//                                      builder5.Build(),
//                                  };
//
//            // Act
//            _builder.WithBuilders(ob => ob.Addresses, builder1, builder2, builder3, builder4, builder5);
//            var oldAddressBook = _builder.Build();
//
//            // Assert
//            oldAddressBook.ID.ShouldEqual(123L);
//            oldAddressBook.Addresses.ShouldNotBeNull();
//            oldAddressBook.Addresses.Count.ShouldEqual(5);
//
//            // NOTE: The instances in addressList and addressBook.Addresses, while in
//            //       the same order are actually separate instances with different
//            //       hashcodes. We can't use CollectionAssert because of that
//            //
//            //CollectionAssert.AreEquivalent(addressList, addressBook.Addresses);
//            //
//            // Since we're using List<> as the implementation for IList<>, we can 
//            // rely on the order being the same.
//            for (var i = 0; i < addressList.Count; i++)
//            {
//                oldAddressBook.Addresses[i].IsInUsa.ShouldEqual(addressList[i].IsInUsa);
//                oldAddressBook.Addresses[i].Latitude.ShouldEqual(addressList[i].Latitude);
//                oldAddressBook.Addresses[i].Longitude.ShouldEqual(addressList[i].Longitude);
//                oldAddressBook.Addresses[i].LotNumber.ShouldEqual(addressList[i].LotNumber);
//                oldAddressBook.Addresses[i].LotSquareFootage.ShouldEqual(addressList[i].LotSquareFootage);
//                oldAddressBook.Addresses[i].StreetAddress_1.ShouldEqual(addressList[i].StreetAddress_1);
//                oldAddressBook.Addresses[i].StreetAddress_2.ShouldEqual(addressList[i].StreetAddress_2);
//                oldAddressBook.Addresses[i].StreetNumber.ShouldEqual(addressList[i].StreetNumber);
//            }
//        }
//    }
//
//    public class OldAddressBookBuilder : TestDataBuilder<OldAddressBook, OldAddressBookBuilder>
//    {
//        public OldAddressBookBuilder()
//        {
//            With(ab => ab.ID, 123L);
//        }
//    }
//}