using Tests.TestingDomain;

namespace Tests.TestDataBuilders
{
    internal class AddressBuilder :
        UnitTestTestDataBuilder<Address, AddressBuilder>
    {
        public AddressBuilder()
        {
            With(a => a.StreetAddress_1, TestDataBuilderSpec._defaultStreetAddress1);
            With(a => a.StreetAddress_2, TestDataBuilderSpec._defaultStreetAddress2);
            With(a => a.StreetNumber, TestDataBuilderSpec._defaultStreetNumber);
            With(a => a.LotSquareFootage, TestDataBuilderSpec._defaultLotSquareFootage);
            With(a => a.IsInUsa, TestDataBuilderSpec._defaultIsInUsa);
            With(a => a.Latitude, TestDataBuilderSpec._defaultLatitude);
            With(a => a.Longitude, TestDataBuilderSpec._defaultLongitude);
            With(a => a.LotNumber, TestDataBuilderSpec._defaultLotNumber);
            With(a => a.ID, TestDataBuilderSpec._defaultAddressId);
            With(a => a.IsNew, TestDataBuilderSpec._defaultAddressIsNew);
        }
    }
}