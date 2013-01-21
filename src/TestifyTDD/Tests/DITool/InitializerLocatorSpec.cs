using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using Rhino.Mocks;
using NBehave.Spec.NUnit;
using TestifyTDD.DITool;
using System.Linq;

namespace Tests.DITool
{
    [TestFixture]
    public class InitializerLocatorSpec : TestBase
    {
        [Test]
        public void FindInitializersInAppDomain_should_find_types_and_return_instances_of_IInitializers()
        {
            // Arrange
            var assembly = Assembly.GetAssembly(GetType());
            var initializerStubType = typeof (InitializerStub);
            var initializerDummyType = typeof (InitializerDummy);

            var initializerTypeList = new InitializerTypeList()
                                          {
                                              initializerStubType,
                                              initializerDummyType
                                          };

            var filter = M<IInitializerFilter>();

            // return the initializerTypeList above for this assembly
            filter
                .Expect(f => f.FindInitializers(assembly))
                .Return(initializerTypeList)
                .Repeat.Any();

            // return an empty InitializerTypeList above for all other assemblies
            filter
                .Expect(f => f.FindInitializers(null))
                .IgnoreArguments()
                .Return(new InitializerTypeList())
                .Repeat.Any();
                
            var locator = new InitializerLocator(filter);

            // Act
            var resultList = locator.FindInitializersInAppDomain();

            // Assert
            filter.VerifyAllExpectations();
            resultList[0].GetType().ShouldEqual(initializerStubType);
            resultList[1].GetType().ShouldEqual(initializerDummyType);
        }

        [Test]
        public void GetFilteredAssemblies_should_filter_out_uninteresting_assemblies()
        {
            // Arrange
            var appDomain = AppDomain.CurrentDomain;

            var ignoreList = new[]
                    {
                        "JetBrains.ReSharper.TaskRunnerFramework",
                        "JetBrains.ReSharper.UnitTestRunner.nUnit",
                        "mscorlib",
                        "NBehave.Spec.NUnit",
                        "nunit.core",
                        "nunit.core.interfaces",
                        "nunit.framework",
                        "nunit.util",
                        "System",
                        "System.Configuration",
                        "System.Core",
                        "System.Xml"
                    };

            // Act
            var filteredAssemblies = appDomain.GetFilteredAssemblies();

            // Acquire
            var foundList = from assembly in filteredAssemblies
                            select assembly.GetName().Name;
            // Assert
            foreach (var assemblyName in foundList)
            {
                Debug.WriteLine(assemblyName);
                ignoreList.ShouldNotContain(assemblyName);
            }
        }
    }
}
