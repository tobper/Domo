using Domo.DI.Caching;
using Domo.DI.Construction;

namespace Domo.DI.Activation
{
    public class SingletonFactoryActivator : FactoryActivator
    {
        public SingletonFactoryActivator(ServiceIdentity identity, IFactory factory)
            : base(identity, factory)
        {
        }

        protected override IServiceCache GetServiceCache(IInjectionContext context)
        {
            return context.Container.ServiceLocator.Resolve<IServiceCache>();
        }
    }
}