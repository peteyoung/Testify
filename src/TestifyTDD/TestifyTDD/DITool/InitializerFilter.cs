using System;
using System.Collections.Generic;
using System.Reflection;

namespace TestifyTDD.DITool
{
    public interface IInitializerFilter
    {
        InitializerTypeList FindInitializers(Assembly assembly);
    }

    public class InitializerFilter : IInitializerFilter
    {
        public InitializerTypeList FindInitializers(Assembly assembly)
        {
            var initializers = new InitializerTypeList();
            
            foreach (var type in assembly.GetTypes())
                if (type.IsConcrete() && type.IsInitializer())
                    initializers.Add(type);

            return initializers;
        }
    }

    internal static class TypeExtensionForInitializers
    {
        public static bool IsInitializer (this Type type)
        {
            return type.GetInterface(typeof (IInitializer).Name) != null;
        }
    }
}
