using System;
using System.Collections;
using System.Collections.Generic;
using TestifyTDD.DITool;

namespace TestifyTDD
{
    public class TypeMapper : ITypeMapper
    {
        private Dictionary<Type, Type> _typeMap = new Dictionary<Type, Type>();

        public void Map<TINTERFACE, TTYPE>()
        {
            Map(typeof(TINTERFACE), typeof(TTYPE));
        }

        public void Map(Type interface_, Type type)
        {
            // TODO: handle abstract classes

            if (! interface_.IsInterface)
                throw new ArgumentException(string.Format(
                    "interface_ must actually be a type that is an interface. {0} is a concrete type",
                    interface_.Name));
            
            _typeMap.Add(interface_, type);
        }

        public Type Resolve<TTYPE>()
        {
            return Resolve(typeof(TTYPE));
        }

        public Type Resolve(Type typeToResolve)
        {            
            // If we have an instantiable type just return it.
            if (! typeToResolve.IsInterface)
                return typeToResolve;

            // Based on interface, get either a non-generic type or
            // a generic type definition
            var typeKey = GetTypeKey(typeToResolve);

            if (!_typeMap.ContainsKey(typeKey))
                throw new ApplicationException(string.Format(
                    "There is no entry mapped for interface {0}",
                    typeKey.Name));

            var mappedType = _typeMap[typeKey];

            // Non-generic types and get returned at this point.
            if (! mappedType.IsGenericTypeDefinition)
                return mappedType;

            // Open generic type definitions get returned at this point.
            if (typeToResolve.ContainsGenericParameters)
                return mappedType;

            // We need to create an instantiable generic type based on the 
            // generic type definition's type, and the generic arguments from
            // the type we're mapping from.
            var instantiableGenericType =
                mappedType.MakeGenericType(typeToResolve.GetGenericArguments());

            return instantiableGenericType;
        }

        private Type GetTypeKey(Type typeToResolve)
        {
            if (! typeToResolve.IsInterface)
                throw new ArgumentException("typeToResolve must be an interface");

            if (typeToResolve.IsGenericType)
                return typeToResolve.GetGenericTypeDefinition();

            return typeToResolve;
        }
    }
}
