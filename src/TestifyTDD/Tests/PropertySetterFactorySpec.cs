using NUnit.Framework;
using NBehave.Spec.NUnit;
using TestifyTDD.PropertySetters;
using Tests.TestDataBuilders;
using Tests.TestingDomain;

namespace Tests
{
    [TestFixture]
    public class PropertySetterFactorySpec
    {
        [Test]
        public void Should_get_PassthroughPropertySetter_with_plain_object()
        {
            // Arrange
            var value = new object();
            var factory = new PropertySetterFactory();

            // Act
            var propertySetter = factory.GetPropertySetter<object>(value);

            // Assert
            propertySetter.ShouldNotBeNull();
            propertySetter.GetType().ShouldBeTheSameAs(typeof(PassthroughPropertySetter<object>));
        }

        [Test]
        public void Should_get_TestDataBuilderPropertySetter_with_ITestDataBuilder_object()
        {
            // Arrange
            var value = new AddressBuilder();
            var factory = new PropertySetterFactory();

            // Act
            var propertySetter = factory.GetPropertySetter<Address>(value);

            // Assert
            propertySetter.ShouldNotBeNull();
            propertySetter.GetType().ShouldBeTheSameAs(typeof(TestDataBuilderPropertySetter<Address>));
        }
    }
}
