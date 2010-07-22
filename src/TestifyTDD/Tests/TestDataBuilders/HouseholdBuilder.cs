using TestifyTDD;
using Tests.TestingDomain;

namespace Tests.TestDataBuilders
{
    public class HouseholdBuilder :
        TestDataBuilder<Household, HouseholdBuilder>
    {
        public HouseholdBuilder()
        {
            var addressBuilder = new AddressBuilder();
            
            With(c => c.ID, TestDataBuilderSpec._defaultHouseholdId);
            With(c => c.IsNew, TestDataBuilderSpec._defaultHouseholdIsNew);
            With(c => c.FamilyName, TestDataBuilderSpec._defaultFamilyName);
            With(c => c.Address, addressBuilder.Build());
            Withs(c => c.Neighbors,
                  addressBuilder.With(a => a.StreetNumber,
                                      TestDataBuilderSpec._defaultStreetNumber - 2),
                  addressBuilder.With(a => a.StreetNumber,
                                      TestDataBuilderSpec._defaultStreetNumber + 2));
        }
    }
}