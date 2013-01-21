using TestifyTDD.DITool;

namespace Tests.DITool
{
    internal class InitializerStub : IInitializer
    {
        public ITypeMapper TypeMapper { get; private set; }
        
        public void InitializeTypeMapper(ITypeMapper typeMapper)
        {
            TypeMapper = typeMapper;
        }
    }
}