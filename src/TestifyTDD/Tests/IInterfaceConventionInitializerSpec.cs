using NUnit.Framework;
using Rhino.Mocks;
using NBehave.Spec.NUnit;
using TestifyTDD;
using TestifyTDD.DITool;
using TestifyTDD.Helpers;
using TestifyTDD.PropertySetters;

namespace Tests
{
    [TestFixture]
    public class IInterfaceConventionInitializerSpec : TestBase
    {
        [Test]
        public void InitializeTypeMapper_should_call_Map_with_initializers_found_in_assembly()
        {
            // Arrange
            var initializer = new IInterfaceConventionInitializer();
            var typeMapper = M<ITypeMapper>();

            typeMapper.Expect(tm => tm.Map(typeof (ITypeMapper), typeof (TypeMapper)));
            typeMapper.Expect(tm => tm.Map(typeof (IInitializerFilter), typeof (InitializerFilter)));
            typeMapper.Expect(tm => tm.Map(typeof(IInitializerLocator), typeof(InitializerLocator)));
            typeMapper.Expect(tm => tm.Map(typeof(IPropertySetterFactory), typeof(PropertySetterFactory)));
            //typeMapper.Expect(tm => tm.Map(typeof(), typeof()));            

            typeMapper.Expect(tm => tm.Map(typeof (IPropertyHelper<>), typeof (PropertyHelper<>)));
            typeMapper.Expect(tm => tm.Map(typeof (ITestDataBuilder<,>), typeof (TestDataBuilder<,>)));
            //typeMapper.Expect(tm => tm.Map(typeof (<>), typeof (<>)));

            // Act
            initializer.InitializeTypeMapper(typeMapper);
            
            // Assert
            typeMapper.VerifyAllExpectations();
        }
    }
}
