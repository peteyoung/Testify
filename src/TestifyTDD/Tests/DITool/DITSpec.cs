using System;
using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using NBehave.Spec.NUnit;
using TestifyTDD;
using TestifyTDD.DITool;
using TestifyTDD.Helpers;
using TestifyTDD.PropertySetters;

namespace Tests.DITool
{
    [TestFixture]
    public class DITSpec : TestBase 
    {
        [Test]
        public void CreateInstance_should_search_for_initializers_when_typeMapper_uninitialized()
        {
            // Arrange
            var typeMapper = S<ITypeMapper>();

            var filter = S<IInitializerFilter>();

            var locator = M<IInitializerLocator>();
            locator.Expect(loc => loc.FindInitializersInAppDomain())
                .Return(new List<IInitializer>());

            var ditDependencyCtor = new DITDependencyConstructor
                                        {
                                            TypeMapper = () => typeMapper,
                                            InitializerFilter = () => filter,
                                            InitializerLocator = f => locator
                                        };
            
            DITDependencyCtor.Override(ditDependencyCtor);

            var dit = new DIT();

            // Act
            dit.CreateInstance(typeof (Type));

            // Assert
            locator.VerifyAllExpectations();
        }

        [Test]
        public void CreateInstance_should_fire_initializers_when_typeMapper_unitialized()
        {
            // Arrange
            var typeMapper = S<ITypeMapper>();

            var filter = S<IInitializerFilter>();

            var initializer_1 = M<IInitializer>();
            initializer_1.Expect(i => i.InitializeTypeMapper(typeMapper));

            var initializer_2 = M<IInitializer>();
            initializer_2.Expect(i => i.InitializeTypeMapper(typeMapper));

            var locator = S<IInitializerLocator>();
            locator.Expect(loc => loc.FindInitializersInAppDomain())
                .Return(new List<IInitializer> { initializer_1, initializer_2 });

            var ditDependencyCtor = new DITDependencyConstructor
            {
                TypeMapper = () => typeMapper,
                InitializerFilter = () => filter,
                InitializerLocator = f => locator
            };

            DITDependencyCtor.Override(ditDependencyCtor);

            var dit = new DIT();


            // Act
            dit.CreateInstance<Type>();

            // Assert
            initializer_1.VerifyAllExpectations();
            initializer_2.VerifyAllExpectations();
        }

        #region Integration Tests

        [Test]
        public void Should_get_proper_mapping_to_types_for_interfaces()
        {
            // Arrange
            var initializer = new IInterfaceConventionInitializer();
            var typeMapper = new TypeMapper();

            // Act
            initializer.InitializeTypeMapper(typeMapper);

            // Assert
            typeMapper.Resolve(typeof (ITypeMapper)).ShouldEqual(typeof (TypeMapper));
            typeMapper.Resolve(typeof (IInitializerFilter)).ShouldEqual(typeof (InitializerFilter));
            typeMapper.Resolve(typeof (IInitializerLocator)).ShouldEqual(typeof (InitializerLocator));
            typeMapper.Resolve(typeof (IPropertySetterFactory)).ShouldEqual(typeof (PropertySetterFactory));

            typeMapper.Resolve(typeof (IPropertyHelper<string>)).ShouldEqual(typeof (PropertyHelper<string>));
            typeMapper.Resolve(typeof (IPropertyHelper<Int64>)).ShouldEqual(typeof (PropertyHelper<Int64>));
            typeMapper.Resolve(typeof (IPropertyHelper<>)).ShouldEqual(typeof (PropertyHelper<>));
        }

        #endregion Integration Tests

    }

    public class DITDependencyCtor : DIT
    {
        public static void Override(DITDependencyConstructor create)
        {
            Create = create;
        }
    }
}

// NOTE: I'm preserving this test to remind myself that you CAN get stupid with a mocking framework
/*

        [Test]
        public void CreateInstance_should_return_the_desired_instance()
        {
            // Arrange
            var typeInterfaceIList = typeof (IList);
            var typeConcreteArrayList = typeof (ArrayList);

            var typeInterfaceIListGeneric = typeof (IList<>);
            var typeDefGenericList = typeof (List<>);

            var typeMapper = M<ITypeMapper>();

            typeMapper.Expect(tm => tm.Resolve(typeInterfaceIList))
                .Return(typeConcreteArrayList)
                .Repeat.Any();

            typeMapper.Expect(tm => tm.Resolve(typeInterfaceIListGeneric))
                .Return(typeDefGenericList)
                .Repeat.Any();

            // Simulate the functionality of a real IInintializer
            // using a lambda expression. This is almost like an anonymous
            // class in Java.
            var initializer = S<IInitializer>();
            initializer.Expect(i => i.InitializeTypeMapper(typeMapper))
                .Do(
                    (Action<ITypeMapper>)
                        (tm =>
                             {
                                 tm.Map(typeInterfaceIList, typeConcreteArrayList);
                                 tm.Map(typeInterfaceIListGeneric, typeDefGenericList);
                             }));

            var initializerTypeList = new InitializerTypeList
                                          {
                                              initializer
                                          };

            typeMapper.Expect(tm => tm.Map(typeInterfaceIList, typeConcreteArrayList));

            var filter = S<IInitializerFilter>();

            var locator = S<IInitializerLocator>();
            locator.Expect(loc => loc.FindInitializersInAppDomain())
                .Return(initializerTypeList);

            var dit = new DITFake(typeMapper, filter, locator);

            // Act
            var foundType = dit.CreateInstance(typeInterfaceIList);

            // Assert
            foundType.ShouldNotBeNull();
            foundType.ShouldEqual(typeConcreteArrayList);
        }
*/