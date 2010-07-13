using System;

namespace TestifyTDD
{
    public interface ICollectionTypeMapper
    {
        void Map(Type interface_, Type type);
        Type Resolve(Type typeToResolve);
    }
}