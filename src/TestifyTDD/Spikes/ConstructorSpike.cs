using System;
using System.Reflection;
using NUnit.Framework;
using Tests.TestDataBuilders;

namespace Spikes
{
    [TestFixture]
    public class ConstructorSpike
    {
        [Test]
        public void FindConstructor()
        {
            // Arrange
            var addressBuilderType = typeof (AddressBuilder);
            var bindingFlags = BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance;
            var constructors = addressBuilderType.GetConstructors(bindingFlags);

            // Act

            // Assert
            Console.WriteLine(string.Format("{0} constructors found.",  constructors.Length));

        }
    }
}
