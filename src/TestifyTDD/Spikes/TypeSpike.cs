using System;
using System.Collections.Generic;
using System.Reflection;
using NUnit.Framework;

namespace Spikes
{
    [TestFixture]
    public class TypeSpike
    {
        [Test]
        public void ExamineGetInterfaces()
        {
            var fooType = typeof (Foo<object>);

            var fooType2 = Assembly.GetAssembly(GetType()).GetType("Foo'1");

//            var fooInterface = fooType.GetInterface(typeof(IFoo).Name);
            var fooInterface = fooType.GetInterface("IFoo`1");

            Console.WriteLine(string.Format("Foo Interface Name: {0}", fooInterface.Name));
            Console.WriteLine(string.Format("Foo Interface FullName: {0}", fooInterface.FullName));

        }


    }

    internal interface IFoo<T>
    {
        void DoFoo(); 
    }

    internal class Foo<T> : IFoo<T>
    {
        public void DoFoo()
        {
            return;
        }
    }
}