using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using System.Linq.Expressions;

namespace Spikes.DI
{
    [TestFixture]
    public class DISpike
    {
        [Test]
        public void Get_current_assembly()
        {
            var assembly = Assembly.GetAssembly(typeof(DISpike));

            Debug.WriteLine(assembly.GetName());
        }

        [Test]
        public void Get_current_assemblys_members()
        {
            var assembly = Assembly.GetAssembly(typeof(DISpike));

            foreach (var module in assembly.GetModules())
                Debug.WriteLine("module: " + module.FullyQualifiedName);

            foreach (var type in assembly.GetTypes())
                Debug.WriteLine(
                    string.Format("type: {0}\tis interface: {1}",
                                  type.Name,
                                  type.IsInterface));
        }

        [Test]
        public void Get_current_assemblys_types()
        {
            var assembly = Assembly.GetAssembly(typeof(DISpike));

            foreach (var type in assembly.GetTypes())
                Debug.WriteLine(
                    string.Format("type: {0}\tis interface: {1}\tFQN: {2}",
                                  type.Name,
                                  type.IsInterface,
                                  type.FullName));
        }

        [Test]
        public void Get_current_assemblys_interfaces()
        {
            var assembly = Assembly.GetAssembly(typeof(DISpike));

            foreach (var type in assembly.GetTypes())
                if (type.IsInterface)
                    Debug.WriteLine(
                        string.Format("interface: {0}",
                                      type.Name));
        }

        [Test]
        public void Get_current_assemblys_interfaces_using_linq()
        {
            var assembly = Assembly.GetAssembly(typeof(DISpike));

            var interfaces = from type in assembly.GetTypes()
                             where type.IsInterface
                             select type;

            foreach (var intrface in interfaces)
                Debug.WriteLine(
                    string.Format("interface: {0}",
                                  intrface.Name));
        }

        [Test]
        public void Get_current_assemblys_types_matching_default_convention()
        {
            var assembly = Assembly.GetAssembly(typeof(DISpike));

            foreach (var type in assembly.GetConcreteTypes())
            {
                var interfaceName = "I" + type.Name;
                var intrface = type.GetInterface(interfaceName);

                if (intrface != null)
                    Debug.WriteLine(
                            string.Format("interface: {0}\timplementation: {1}",
                                          intrface.Name,
                                          type.Name));
            }
        }
    }

    public static class AssemblyExtensions
    {
        public static Type[] GetConcreteTypes(this Assembly assembly)
        {
            return Array.FindAll(
                assembly.GetTypes(),
                type => !(type.IsAbstract || type.IsInterface));
        }
    }
}