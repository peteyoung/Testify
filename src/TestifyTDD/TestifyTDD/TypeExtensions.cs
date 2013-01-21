using System;

namespace TestifyTDD
{
    public static class TypeExtensions
    {
        public static bool IsConcrete(this Type type)
        {
            return !(type.IsAbstract || type.IsInterface);
        }

        public static bool Implements(this Type type, Type intrface)
        {
            return Implements(type, intrface.Name);
        }

        public static bool Implements(this Type type, string interfaceName)
        {
            return type.GetInterface(interfaceName) != null;
        }
    }
}
