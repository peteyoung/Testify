using System;
using System.Linq.Expressions;
using System.Reflection;

namespace TestifyTDD.Helpers
{
    public class PropertyHelper<TDOMAIN> : IPropertyHelper<TDOMAIN>
    {
        private const BindingFlags BINDING_FLAGS = BindingFlags.Instance | BindingFlags.Public;
        
        // http://stackoverflow.com/questions/491429/how-to-get-the-propertyinfo-of-a-specific-property
        public PropertyInfo GetPropertyInfo<TRETURNS>(Expression<Func<TDOMAIN, TRETURNS>> property)
        {
            Expression body = property;

            if (body is LambdaExpression)
                body = ((LambdaExpression)body).Body;

            if (body.NodeType != ExpressionType.MemberAccess)
                throw new ArgumentException(string.Format(
                                                "\"{0}\" is not a property. Please specify a property on the type {1}", 
                                                body,
                                                typeof(TDOMAIN).Name));

            var memberExpression = (MemberExpression)body;
            var member = memberExpression.Member;

            // Sometimes a property may exist on a superclass instead of the one we're
            // really interested in. Here is where we cast down to the subclass.
            var propertyInfo = CastPropertyInfoDown((PropertyInfo)member);

            return propertyInfo;
        }

        // http://www.codeproject.com/KB/cs/JTConvert.aspx
        private PropertyInfo CastPropertyInfoDown(PropertyInfo propertyInfo)
        {
            var declaringType = propertyInfo.DeclaringType;

            if (declaringType == typeof(TDOMAIN))
                return propertyInfo;

            if (typeof(TDOMAIN).IsSubclassOf(declaringType))
            {
                var propDerived = typeof(TDOMAIN).GetProperty(propertyInfo.Name, BINDING_FLAGS);
                return propDerived;
            }

            throw new ArgumentException(string.Format(
                                            "\"{0}\" is not a super type of \"{1}\". Unable to down cast propertyInfo to proper type",
                                            declaringType.Name,
                                            typeof(TDOMAIN).Name));
        }

        // http://weblogs.asp.net/marianor/archive/2009/04/10/using-expression-trees-to-get-property-getter-and-setters.aspx
        public Action<TDOMAIN, object> GetValueSetter(PropertyInfo propertyInfo)
        {
            if (!(typeof(TDOMAIN) == propertyInfo.DeclaringType || typeof(TDOMAIN).IsSubclassOf(propertyInfo.DeclaringType)))
                throw new ArgumentException(string.Format(
                                                "trying to set {0}.{1} when type is actually {2}",
                                                propertyInfo.DeclaringType.Name,
                                                propertyInfo.Name,
                                                typeof(TDOMAIN).Name));

            // ReflectedType should have been cast down to TDOMAIN in GetPropertyInfo 
            // if the DeclaringType is a super-type of TDOMAIN
            var instance = Expression.Parameter(propertyInfo.ReflectedType, "i");

            var argument = Expression.Parameter(typeof(object), "a");
            var setMethod = propertyInfo.GetSetMethod(true); // true brings back non-public setters
            var argumentCast = Expression.Convert(argument, propertyInfo.PropertyType);

            if (setMethod == null)
                throw new ArgumentException(string.Format(
                                                "{0}.{1} seems to be missing a setter altogether. No public, protected, or private setter found.",
                                                typeof(TDOMAIN).Name,
                                                propertyInfo.Name));

            var setterCall = Expression.Call(instance, setMethod, argumentCast);
            var setterLambda = Expression.Lambda(setterCall, instance, argument).Compile();

            var action = (Action<TDOMAIN, object>) setterLambda;
            return action;
        }
    }
}