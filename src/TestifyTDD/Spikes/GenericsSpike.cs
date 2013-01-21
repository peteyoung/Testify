using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;

namespace Spikes
{
    [TestFixture]
    public class GenericsSpike
    {
        [Test]
        public void Poke_around_generics()
        {
            var nonGenericType = typeof(IList);
            var genericType = typeof(IList<string>);
            var genericTypeDef = typeof(IList<>);

            ShowIsGeneric(nonGenericType);
            ShowIsGenericDef(nonGenericType);
            ShowGenericArguments(nonGenericType); 

            ShowIsGeneric(genericType);
            ShowIsGenericDef(genericType);
            ShowGenericArguments(genericType); 

            ShowIsGeneric(genericTypeDef);
            ShowIsGenericDef(genericTypeDef);
            ShowGenericArguments(genericTypeDef);
        }

        public void ShowIsGeneric(Type type)
        {
            Console.WriteLine(
                string.Format("{0} is {1}a generic type",
                type.Name,
                (type.IsGenericType) ? string.Empty : "not "));
        }

        public void ShowIsGenericDef(Type type)
        {
            Console.WriteLine(
                string.Format("{0} is {1}a generic type definition",
                type.Name,
                (type.IsGenericType) ? string.Empty : "not "));
        }

        public void ShowGenericArguments(Type type)
        {
            Console.WriteLine(
                string.Format("{0}'s Generic Arguments:", type.Name));

            foreach (var param in type.GetGenericArguments())
                Console.WriteLine(
                    string.Format("\t{0}\tis parameter? {1}", 
                                    param.Name,
                                    param.IsGenericParameter));
        }

        public void ShowContainsGenericParameters(Type type)
        {
            Console.WriteLine(
                string.Format("{0} Contains Generic Argument: {1}", 
                    type.Name,
                    type.ContainsGenericParameters));
        }

        [Test]
        public void Capture_open_generic_parameter_types()
        {
            var genericTypeDef_1 = typeof(IDictionary<,>);
//            var genericTypeDef_2 = typeof(IDictionary<string,>);
            var genericTypeDef_3 = typeof(IDictionary<Int32, Double>);

            var param_1 = genericTypeDef_1.GetGenericArguments()[0];
            var param_2 = genericTypeDef_1.GetGenericArguments()[1];

            ShowGenericArguments(genericTypeDef_1);
            ShowContainsGenericParameters(genericTypeDef_1);

//            ShowGenericParameters(genericTypeDef_2);
//            ShowContainsGenericParameters(genericTypeDef_2);

            ShowGenericArguments(genericTypeDef_3);
            ShowContainsGenericParameters(genericTypeDef_3);
        }

        [Test]
        public void Does_open_generic_have_generic_parameters()
        {
            var closedGenericType = typeof (IDictionary<string, int>);
//            var semiGenericType = typeof(IDictionary<string,T>);
            var openGenericType = typeof(IDictionary<,>);

            Console.WriteLine(
                string.Format("closed generic contains generic parameters: {0}",
                                closedGenericType.ContainsGenericParameters));

//            Console.WriteLine(
//                string.Format("semi closed generic contains generic parameters: {0}",
//                                semiGenericType.ContainsGenericParameters));

            Console.WriteLine(
                string.Format("open generic contains generic parameters: {0}",
                                openGenericType.ContainsGenericParameters));
        }
    }
}
