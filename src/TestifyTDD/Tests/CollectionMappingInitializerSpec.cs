using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using NBehave.Spec.NUnit;
using TestifyTDD;
using TestifyTDD.DITool;

namespace Tests
{
    [TestFixture]
    public class CollectionMappingInitializerSpec : TestBase
    {
        [Test]
        public void InitializeTypeMapper_should_call_Map_on_TypeMapper()
        {
            // Arrange
            var initializer = new CollectionMappingInitializer();
            var typeMapper = M<ITypeMapper>();

            typeMapper.Expect(tm => tm.Map<IList, ArrayList>());
            typeMapper.Expect(tm => tm.Map<IDictionary, Hashtable>());
            typeMapper.Expect(tm => tm.Map(typeof(IList<>), typeof(List<>)));
            typeMapper.Expect(tm => tm.Map(typeof(IDictionary<,>), typeof(Dictionary<,>)));

            // Act
            initializer.InitializeTypeMapper(typeMapper);

            // Assert
            typeMapper.VerifyAllExpectations();
        }

        // NOTE: Integration test
        [Test]
        public void InitializeTypeMapper_should_configure_TypeMapper_to_resolve_types()
        {
            // Arrange
            var initializer = new CollectionMappingInitializer();
            var typeMapper = new TypeMapper();

            // Act
            initializer.InitializeTypeMapper(typeMapper);

            // Assert
            typeMapper.Resolve<IList>().ShouldEqual(typeof(ArrayList));
            typeMapper.Resolve<IDictionary>().ShouldEqual(typeof(Hashtable));
            typeMapper.Resolve<IList<string>>().ShouldEqual(typeof(List<string>));
            typeMapper.Resolve<IDictionary<int, double>>()
                .ShouldEqual(typeof(Dictionary<int, double>));

            Assert.Throws<ApplicationException>(
                () => typeMapper.Resolve<IEnumerable>());

            typeMapper.Resolve(typeof(IList<>)).ShouldEqual(typeof(List<>));
            typeMapper.Resolve(typeof(IDictionary<,>)).ShouldEqual(typeof(Dictionary<,>));
        }
    }
}
