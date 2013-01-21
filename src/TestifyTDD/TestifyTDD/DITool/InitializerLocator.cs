using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;

namespace TestifyTDD.DITool
{
    public interface IInitializerLocator
    {
        List<IInitializer> FindInitializersInAppDomain();
        List<IInitializer> FindInitializersInAppDomain(AppDomain appDomain);
    }

    public class InitializerLocator : IInitializerLocator
    {
        private readonly IInitializerFilter _filter;

        public InitializerLocator(IInitializerFilter filter)
        {
            _filter = filter;
        }

        public List<IInitializer> FindInitializersInAppDomain()
        {
            return FindInitializersInAppDomain(AppDomain.CurrentDomain);
        }

        public List<IInitializer> FindInitializersInAppDomain(AppDomain appDomain)
        {
            var initializerTypeList = new InitializerTypeList();
            var initializerList = new List<IInitializer>();

            foreach (var assembly in appDomain.GetFilteredAssemblies())
                initializerTypeList.AddRange(
                    _filter.FindInitializers(assembly));

            foreach (var initializerType in initializerTypeList)
                initializerList.Add( 
                    (IInitializer)Activator.CreateInstance(initializerType));

            return initializerList;
        }
    }

    public static class AppDomainExtensions
    {
//        public static Assembly[] GetFilteredAssemblies(this AppDomain appDomain)
//        {
//            //var ignoreList = new Assembly[]{};
//            var ignoreList = new[]
//                    {
//                        Assembly.GetAssembly(typeof(AppDomainExtensions))
//                    };
//
//            var foundList = appDomain.GetAssemblies()
//                .Except(ignoreList);
//
//            return foundList.ToArray();
//        }

        public static Assembly[] GetFilteredAssemblies(this AppDomain appDomain)
        {
            var ignoreList = new[]
                    {
                        "JetBrains.ReSharper.TaskRunnerFramework",
                        "JetBrains.ReSharper.UnitTestRunner.nUnit",
                        "mscorlib",
                        "nunit.core",
                        "nunit.core.interfaces",
                        "nunit.framework",
                        "nunit.util",
                        "NBehave.Spec.NUnit",
                        "System",
                        "System.Configuration",
                        "System.Core",
                        "System.Xml"
                    };

            var foundList = from assembly in appDomain.GetAssemblies()
                            where !ignoreList.Contains(assembly.GetName().Name)
                            select assembly;

            return foundList.ToArray();
        }
    }
}
