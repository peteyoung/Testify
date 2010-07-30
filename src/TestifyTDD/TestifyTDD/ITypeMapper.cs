using System;

namespace TestifyTDD
{
    public interface ITypeMapper
    {
        void Map(Type interface_, Type type);
        Type Resolve(Type typeToResolve);
    }
}