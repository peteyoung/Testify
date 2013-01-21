using System;

namespace TestifyTDD.DITool
{
    public class DITDependencyConstructor
    {
        public Func<ITypeMapper> TypeMapper =
            () => new TypeMapper();

        public Func<IInitializerFilter> InitializerFilter =
            () => new InitializerFilter();

        public Func<IInitializerFilter, IInitializerLocator> InitializerLocator =
            filter => new InitializerLocator(filter);
    }
}
