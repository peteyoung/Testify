using System;
using System.Reflection;

namespace TestifyTDD.DITool
{
    public static class AssemblyExtensions
    {
        public static Type[] GetConcreteTypes(this Assembly assembly)
        {
            return Array.FindAll(
                assembly.GetTypes(),
                type => type.IsConcrete());
        }
    }
}
