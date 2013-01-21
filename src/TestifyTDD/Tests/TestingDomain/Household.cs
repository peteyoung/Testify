using System.Collections.Generic;

namespace Tests.TestingDomain
{
    public class Household : UnitTestDomainBase
    {
        public string FamilyName { get; set; }
        public Address Address { get; set; }
        public IList<Address> Neighbors { get; set; }
    }
}