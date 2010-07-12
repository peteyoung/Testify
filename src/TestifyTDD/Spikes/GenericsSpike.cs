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

            ShowIsGeneric(genericType);
            ShowIsGenericDef(genericType);

            ShowIsGeneric(genericTypeDef);
            ShowIsGenericDef(genericTypeDef);
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
    }
}
