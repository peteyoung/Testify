using System;

namespace TestifyTDD.DITool
{
    public interface ITypeMapper
    {
        void Map<TINTERFACE, TTYPE>();
        void Map(Type interface_, Type type);
        Type Resolve<TTYPE>();
        Type Resolve(Type typeToResolve);
    }
}