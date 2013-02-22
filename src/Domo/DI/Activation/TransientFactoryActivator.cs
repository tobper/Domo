using Domo.DI.Caching;
using Domo.DI.Construction;

namespace Domo.DI.Activation
{
    public class TransientFactoryActivator : FactoryActivator
    {
        public TransientFactoryActivator(ServiceIdentity identity, IFactory factory)
            : base(identity, factory)
        {
        }

        protected override IServiceCache GetServiceCache(IInjectionContext context)
        {
            return null;
        }
    }
}