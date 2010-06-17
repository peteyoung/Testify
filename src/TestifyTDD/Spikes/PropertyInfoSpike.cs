using System.Reflection;
using NUnit.Framework;
using NBehave.Spec.NUnit;

namespace Spikes
{
    [TestFixture]
    public class PropertyInfoSpike
    {
        private const BindingFlags Flags =
            BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic;

        [Test]
        public void Does_getting_PropertyInfo_return_the_same_instance()
        {
            // Arrange
            var cwpType = typeof(ClassWithProperty);

            // Act
            var propertyInfo_1 = cwpType.GetProperty("AProperty", Flags);
            var propertyInfo_2 = cwpType.GetProperty("AProperty", Flags);

            // Assert
            propertyInfo_1.ShouldBeTheSameAs(propertyInfo_2);
            propertyInfo_1.ShouldEqual(propertyInfo_2);
            propertyInfo_1.GetHashCode().ShouldEqual(propertyInfo_2.GetHashCode());
        }

        internal class ClassWithProperty
        {
            public string AProperty { get; set; }
        }
    }
}
