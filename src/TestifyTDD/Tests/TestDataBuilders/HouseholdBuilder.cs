using Tests.TestingDomain;

namespace Tests.TestDataBuilders
{
    internal class HouseholdBuilder :
        UnitTestTestDataBuilder<Household, HouseholdBuilder>
    {
        public HouseholdBuilder()
        {
            With(c => c.ID, TestDataBuilderSpec._defaultHouseholdId);
            With(c => c.IsNew, TestDataBuilderSpec._defaultHouseholdIsNew);
            With(c => c.FamilyName, TestDataBuilderSpec._defaultFamilyName);
            With(c => c.Address, new AddressBuilder().Build());
        }
    }
}