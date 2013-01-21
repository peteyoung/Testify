namespace Spikes.DI
{
    public interface IFoo
    {
    }

    public class Foo : IFoo
    {
    }

    public class Boo : IFoo
    {
    }
}

namespace Spikes.DI.Bar
{
    public interface IBar
    {
    }

    public class Bar : IBar
    {
    }

    public class Baz : IBar
    {
    }
}

namespace Spikes.DI.IFizz
{
    public interface IFizz
    {
    }
}

namespace Spikes.DI.Not.In.I.Fizz
{
    public class Fizz : Spikes.DI.IFizz.IFizz
    {
    }
}

namespace Spikes.DI.Lonely
{
    public interface IAmLonely
    {
    }
}

namespace Spikes.DI.IFozz.Generic
{
    public interface IFozz<T>
    {
        void FozzIt(T fooz);
    }

    public class Fozz<T> : IFozz<T>
    {
        public void FozzIt(T fooz) { }
    }

}
