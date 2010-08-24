using System.Diagnostics;
using System.Reflection;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using TestifyTDD;
using TestifyTDD.DITool;

namespace Tests
{
    [TestFixture]
    public class AssemblyExtensionsSpec
    {
        [Test]
        public void GetConcreteTypes_should_only_return_concrete_types()
        {
            // Arrange
            var assembly = Assembly.GetAssembly(GetType());
            var assemblyName = assembly.GetName().Name;

            // Act
            //var types = assembly.GetTypes(); // this fails
            var types = assembly.GetConcreteTypes();

            // Assert
            foreach (var type in types)
            {
                type.IsInterface.ShouldBeFalse();
                type.IsAbstract.ShouldBeFalse();
                Debug.WriteLine(
                    string.Format(
                        "type {0}.{1} is concrete", 
                        assemblyName,
                        type.Name));
            }
        }
    }

    public interface IDoNothing { void DoNothing(); }
    public abstract class AnIDoNothing : IDoNothing { public void DoNothing(){} }
}
