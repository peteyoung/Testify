using NUnit.Framework;
using NBehave.Spec.NUnit;
using TestifyTDD;

namespace Tests
{
    [TestFixture]
    public class TypeExtensionsSpec
    {
        [Test]
        public void IsConcrete_should_return_true_for_a_concrete_type()
        {
            typeof(ConcreteAmI).IsConcrete().ShouldBeTrue();
        }

        [Test]
        public void IsConcrete_should_return_false_for_an_abstract_type()
        {
            typeof(AbstractAmI).IsConcrete().ShouldBeFalse();
        }

        [Test]
        public void IsConcrete_should_return_false_for_an_interface()
        {
            typeof(IAmAnInterface).IsConcrete().ShouldBeFalse();
        }

        [Test]
        public void Implements_should_return_true_if_type_implements_interface_by_name()
        {
            typeof(ConcreteAmI).Implements(typeof(IAmAnInterface).Name).ShouldBeTrue();
        }

        [Test]
        public void Implements_should_return_false_if_type_does_not_implement_interface_by_name()
        {
            typeof(ConcreteAmI).Implements(typeof(IAmAnontherInterface).Name).ShouldBeFalse();
        }

        [Test]
        public void Implements_should_return_true_if_type_implements_interface()
        {
            typeof(ConcreteAmI).Implements(typeof(IAmAnInterface)).ShouldBeTrue();
        }

        [Test]
        public void Implements_should_return_false_if_type_does_not_implement_interface()
        {
            typeof(ConcreteAmI).Implements(typeof(IAmAnontherInterface)).ShouldBeFalse();
        }
    }

    public interface IAmAnInterface { void Foo(); }
    public interface IAmAnontherInterface { void Baz(); }

    public abstract class AbstractAmI : IAmAnInterface
    {
        public void Foo() { return; }
        public abstract void Bar();
    }

    public class ConcreteAmI : AbstractAmI
    {
        public override void Bar() { return; }
    }
}
