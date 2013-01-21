using System;
using System.Collections;
using System.Collections.Generic;

namespace TestifyTDD.DITool
{
    public class DIT
    {
        // How do you inject into the Dependency Injector itself
        // when your runtime environment has no good entry point? 
        // There's no main() or Global.asax.cs for a suite of Unit
        // Tests. These protected Func<T> properties are inflection
        // points for injecting mocks.
        protected static DITDependencyConstructor Create = new DITDependencyConstructor();

        /*[ThreadStatic]*/ 
        private ITypeMapper _typeMapper;
//        public Type CreateInstance<T>()
//        {
//            return CreateInstance(typeof (T));
//        }

        public T CreateInstance<T>() where T : class
        {
            return CreateInstance(typeof(T)) as T;
        }

        public Type CreateInstance(Type type)
        {
            // lazy initialize the type mappings
            if (_typeMapper == null)
                InitializeTypeMapper();

            // TODO: actually instantiate an instance

            return  _typeMapper.Resolve(type);
        }

        private void InitializeTypeMapper()
        {
            _typeMapper = Create.TypeMapper();

            // Search all assemblies in app domain for IInitializers
            var filter = Create.InitializerFilter();
            var locator = Create.InitializerLocator(filter);
            var initializerList = locator.FindInitializersInAppDomain();

            foreach(var initializer in initializerList)
                initializer.InitializeTypeMapper(_typeMapper);
        }
    }
}

/*
NOTE: 
    IInterfaceConventionInitializer.
    Walk dependency tree through constructors to instantiate.
*/