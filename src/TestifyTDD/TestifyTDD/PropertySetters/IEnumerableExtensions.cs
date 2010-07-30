using System;
using System.Collections;
using System.Reflection;

namespace TestifyTDD.PropertySetters
{
    public static class IEnumerableExtensions
    {
        private static bool RunBuilderTestOnIEnumerable(
            IEnumerable enumerable, 
            Predicate<ITestDataBuilder> builderTest)
        {
            foreach (var mayBeBuilder in enumerable)
            {
                var builder = mayBeBuilder as ITestDataBuilder;

                if (builderTest(builder))
                    return true;
            }

            return false;
        }

        public static bool ContainsBuilders(this IEnumerable enumerable)
        {
            return RunBuilderTestOnIEnumerable(enumerable, b => b != null);
        }

        public static bool ContainsNonBuilders(this IEnumerable enumerable)
        {
            return RunBuilderTestOnIEnumerable(enumerable, b => b == null);
        }

        public static bool IsEmpty(this IEnumerable enumerable)
        {
            foreach (var value in enumerable)
                return false;

            return true;
        }

        public static MethodInfo GetAddMethod(this IEnumerable enumerable)
        {
            // Reflect the Add() method
            var addMethod = enumerable
                                .GetType()
                                .GetMethod("Add",
                                           BindingFlags.Instance | BindingFlags.Public);

            return addMethod;
        }

        public static void InvokeAddMethodWith(this IEnumerable enumerable, object value)
        {
            GetAddMethod(enumerable).Invoke(enumerable, new[] { value });
        }
    }
}
