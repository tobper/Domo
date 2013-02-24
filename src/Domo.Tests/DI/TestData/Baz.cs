namespace Domo.Tests.DI.TestData
{
    public class Baz : IBaz
    {
        public IFoo Foo { get; private set; }
        public IBar Bar { get; private set; }

        public Baz(IFoo foo, IBar bar)
        {
            Foo = foo;
            Bar = bar;
        }
    }
}