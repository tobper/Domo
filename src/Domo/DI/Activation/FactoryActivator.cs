using System;
using Domo.DI.Caching;
using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public abstract class FactoryActivator : IActivator
    {
        private readonly IFactoryContainer _factoryContainer;
        private readonly ITypeSubstitution _typeSubstitution;
        private readonly IInstanceCache _instanceCache;

        protected FactoryActivator(IFactoryContainer factoryContainer, ITypeSubstitution typeSubstitution, IInstanceCache instanceCache)
        {
            _factoryContainer = factoryContainer;
            _typeSubstitution = typeSubstitution;
            _instanceCache = instanceCache;
        }

        public object ActivateService(IInjectionContext context, ServiceIdentity identity)
        {
            var concreteIdentity = GetConcreteIdentity(identity);
            var factoryDelegate = GetFactoryDelegate(context, concreteIdentity.ServiceType);

            if (_instanceCache != null)
                return _instanceCache.Get(concreteIdentity, factoryDelegate);

            return factoryDelegate();
        }

        private ServiceIdentity GetConcreteIdentity(ServiceIdentity identity)
        {
            var concreteType = _typeSubstitution.TryGetConcreteType(identity);
            if (concreteType != null)
                return new ServiceIdentity(concreteType, identity.ServiceName);

            return identity;
        }

        private Func<object> GetFactoryDelegate(IInjectionContext context, Type type)
        {
            return () => _factoryContainer.CreateInstance(type, context);
        }
    }
}