using System.Collections.Generic;

namespace Tests.TestingDomain
{
    public class AddressBook : UnitTestDomainBase
    {
        public IList<Address> Addresses { get; set; }
    }
}