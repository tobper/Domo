using System;
using Domo.DI.Caching;
using Domo.DI.Construction;

namespace Domo.DI.Activation
{
    public abstract class FactoryActivator : IActivator
    {
        public IFactory Factory { get; private set; }
        public ServiceIdentity Identity { get; private set; }

        protected FactoryActivator(ServiceIdentity identity, IFactory factory)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            if (factory == null)
                throw new ArgumentNullException("factory");

            Factory = factory;
            Identity = identity;
        }

        public object GetInstance(IInjectionContext context)
        {
            var instanceCache = GetInstanceCache(context);

            return (instanceCache != null)
                ? instanceCache.Get(Identity, Factory, context)
                : Factory.CreateInstance(context);
        }

        protected abstract IInstanceCache GetInstanceCache(IInjectionContext context);
    }
}