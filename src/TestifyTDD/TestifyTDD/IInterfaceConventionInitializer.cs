using System;
using System.Reflection;
using TestifyTDD.DITool;

namespace TestifyTDD
{
    public class IInterfaceConventionInitializer : IInitializer
    {
        public void InitializeTypeMapper(ITypeMapper typeMapper)
        {
            var thisAssembly = Assembly.GetAssembly(GetType());

            foreach (var type in thisAssembly.GetTypes())
            {
                if (!type.IsConcrete())
                    continue;
                
                var interfaceName = "I" + type.Name;

                var interface_ = type.GetInterface(interfaceName);

                if (interface_ == null)
                    continue;

                // This was tricky to figure out, just getting the interface will get 
                // you the generic type w/o generic parameters, not the generic type 
                // definition.
                var mappedInterface = (interface_.IsGenericType)
                                          ? interface_.GetGenericTypeDefinition()
                                          : interface_;

                var mappedType = (type.IsGenericType)
                                     ? type.GetGenericTypeDefinition()
                                     : type;

                typeMapper.Map(mappedInterface, mappedType);
            }
        }
    }
}
