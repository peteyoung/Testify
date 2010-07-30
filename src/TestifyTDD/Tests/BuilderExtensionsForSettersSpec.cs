using System;
using System.Collections;
using System.Linq.Expressions;
using NUnit.Framework;
using NBehave.Spec.NUnit;
using TestifyTDD;
using TestifyTDD.PropertySetters;

namespace Tests
{
    [TestFixture]
    public class BuilderExtensionsForSettersSpec
    {
        [Test]
        public void InvokeBuildMethod_should_invoke_ITestDataBuilders_Build_method()
        {
            // Arrange
            var builder = new ObjectBuilder();

            // Act
            builder.InvokeBuildMethod();

            // Assert
            builder.Build_WasCalled.ShouldBeTrue();
        }
    }

    internal class ObjectBuilder : ITestDataBuilder<object, ObjectBuilder>
    {
        public bool Build_WasCalled { get; private set; }


        public object Build() { Build_WasCalled = true; return null; }

        public ObjectBuilder With<TRETURNS>(Expression<Func<object, TRETURNS>> property, TRETURNS value) { return null; }

        public ObjectBuilder With<TRETURNS, TBUILDER>(Expression<Func<object, TRETURNS>> property, ITestDataBuilder<TRETURNS, TBUILDER> builder) where TBUILDER : ITestDataBuilder<TRETURNS, TBUILDER>, new() { return null; }

        public ObjectBuilder Withs<TSUBDOMAIN>(Expression<Func<object, IEnumerable>> property, params TSUBDOMAIN[] values) { return null; }

        public ObjectBuilder WithBuilders<TRETURNSCOLLECTION, TBUILDER>(Expression<Func<object, IEnumerable>> property, params ITestDataBuilder<TRETURNSCOLLECTION, TBUILDER>[] builders) { return null; }

        public ObjectBuilder But { get { return null; } }
    }
}
