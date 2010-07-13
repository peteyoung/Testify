using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TestifyTDD.Helpers
{
    public interface IPropertyHelper<TDOMAIN>
    {
        PropertyInfo GetPropertyInfo<TRETURNS>(Expression<Func<TDOMAIN, TRETURNS>> property);
        Action<TDOMAIN, object> GetValueSetter(PropertyInfo propertyInfo);
    }
}