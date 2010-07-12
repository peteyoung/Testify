using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace TestifyTDD
{
    public interface ITestDataBuilder<TDOMAIN, TTHIS>
    {
        TDOMAIN Build();

        TTHIS With<TRETURNS>(Expression<Func<TDOMAIN, TRETURNS>> property, TRETURNS value);

        TTHIS With<TRETURNS, TBUILDER>(
            Expression<Func<TDOMAIN, TRETURNS>> property,
            ITestDataBuilder<TRETURNS, TBUILDER> builder)
            where TBUILDER : ITestDataBuilder<TRETURNS, TBUILDER>, new();

        TTHIS Withs<TRETURNS, TSUBDOMAIN>(
            //Expression<Func<TDOMAIN, TRETURNS>> property,
            Expression<Func<TDOMAIN, IList<TRETURNS>>> property,
            params TSUBDOMAIN[] values);
            //where TRETURNS : IList<TSUBDOMAIN>;

        TTHIS WithBuilders<TRETURNS, TBUILDER>(
            Expression<Func<TDOMAIN, IList<TRETURNS>>> property,
            params ITestDataBuilder<TRETURNS, TBUILDER>[] builders);

        TTHIS But { get; }
    }
}