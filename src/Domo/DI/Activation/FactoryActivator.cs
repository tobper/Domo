using System;
using Domo.DI.Caching;
using Domo.DI.Construction;

namespace Domo.DI.Activation
{
    public abstract class FactoryActivator : IActivator
    {
        private readonly IFactoryContainer _factories;
        private readonly ITypeSubstitution _typeSubstitution;
        private readonly IInstanceCache _instanceCache;

        protected FactoryActivator(IFactoryContainer factories, ITypeSubstitution typeSubstitution, IInstanceCache instanceCache)
        {
            _factories = factories;
            _typeSubstitution = typeSubstitution;
            _instanceCache = instanceCache;
        }

        public object ActivateService(IInjectionContext context, ServiceIdentity identity)
        {
            var concreteIdentity = GetConcreteIdentity(identity);
            var factoryDelegate = GetFactoryDelegate(context, concreteIdentity);

            return (_instanceCache != null)
                ? _instanceCache.Get(concreteIdentity, factoryDelegate)
                : factoryDelegate();
        }

        private ServiceIdentity GetConcreteIdentity(ServiceIdentity identity)
        {
            var concreteType = _typeSubstitution.TryGetConcreteType(identity);
            if (concreteType != null)
                return new ServiceIdentity(concreteType, identity.ServiceName);

            return identity;
        }

        private Func<object> GetFactoryDelegate(IInjectionContext context, ServiceIdentity identity)
        {
            return () => _factories.CreateInstance(identity, context);
        }
    }
}