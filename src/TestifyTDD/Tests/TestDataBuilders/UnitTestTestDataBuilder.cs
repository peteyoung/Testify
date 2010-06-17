using TestifyTDD;
using Tests.TestingDomain;

namespace Tests.TestDataBuilders
{
    public class UnitTestTestDataBuilder<TDOMAIN, TTHIS> : TestDataBuilderBase<TDOMAIN, UnitTestDomainBase, TTHIS>
        where TDOMAIN : UnitTestDomainBase
        where TTHIS : TestDataBuilderBase<TDOMAIN, UnitTestDomainBase, TTHIS>, new()
    {
    }
}