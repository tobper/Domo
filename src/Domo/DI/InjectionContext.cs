using Domo.DI.Activation;

namespace Domo.DI
{
    public class InjectionContext : IInjectionContext
    {
        public IContainer Container { get; private set; }

        public InjectionContext(IContainer container)
        {
            Container = container;
        }
    }
}