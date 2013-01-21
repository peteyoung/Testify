using System;
using System.Collections;
using System.Linq.Expressions;

namespace TestifyTDD
{
    public interface ITestDataBuilder {} // Marker interface for extension methods

    public interface ITestDataBuilder<TDOMAIN, TTHIS> : ITestDataBuilder
    {
        TDOMAIN Build();

        TTHIS With<TRETURNS>(Expression<Func<TDOMAIN, TRETURNS>> property, TRETURNS value);

        TTHIS With<TRETURNS, TBUILDER>(
            Expression<Func<TDOMAIN, TRETURNS>> property,
            ITestDataBuilder<TRETURNS, TBUILDER> builder)
            where TBUILDER : ITestDataBuilder<TRETURNS, TBUILDER>, new();

        TTHIS Withs<TSUBDOMAIN>(
            Expression<Func<TDOMAIN, IEnumerable>> property,
            params TSUBDOMAIN[] values);

        TTHIS WithBuilders<TRETURNSCOLLECTION, TBUILDER>(
            Expression<Func<TDOMAIN, IEnumerable>> property,
            params ITestDataBuilder<TRETURNSCOLLECTION, TBUILDER>[] builders);

        TTHIS But { get; }
    }
}