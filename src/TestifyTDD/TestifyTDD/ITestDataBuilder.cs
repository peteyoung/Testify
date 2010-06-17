using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestifyTDD
{
    public interface ITestDataBuilder<TDOMAIN, TDOMAINBASE, TTHIS>
    {
        TDOMAIN Build();

        TTHIS With<TRETURNS>(Expression<Func<TDOMAIN, TRETURNS>> property, TRETURNS value);

        TTHIS With<TRETURNS, TBUILDER>(
            Expression<Func<TDOMAIN, TRETURNS>> property,
            ITestDataBuilder<TRETURNS, TDOMAINBASE, TBUILDER> builder)
            where TRETURNS : TDOMAINBASE
            where TBUILDER : ITestDataBuilder<TRETURNS, TDOMAINBASE, TBUILDER>, new();

        TTHIS Withs<TRETURNS, TSUBDOMAIN>(
            Expression<Func<TDOMAIN, TRETURNS>> property,
            params TSUBDOMAIN[] values)
            where TSUBDOMAIN : TDOMAINBASE
            where TRETURNS : IList<TSUBDOMAIN>;

        TTHIS But { get; }
    }
}