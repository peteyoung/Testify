using System.Reflection;
using NUnit.Framework;
using TestifyTDD.DITool;

namespace Tests.DITool
{
    [TestFixture]
    public class InitializerFilterSpec
    {
        [Test]
        public void InitializerFilter_should_find_all_initializers_in_assembly()
        {
            // Arrange
            var assembly = Assembly.GetAssembly(GetType());
            var filter = new InitializerFilter();

            // Act
            var initializerTypeList = filter.FindInitializers(assembly);

            // Assert
            CollectionAssert.AreEquivalent(
                initializerTypeList,
                new[]
                    {
                        typeof (InitializerStub), 
                        typeof (InitializerDummy)
                    });
        }
    }
}
