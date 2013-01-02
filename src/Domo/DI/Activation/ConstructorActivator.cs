using Domo.DI.Caching;
using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public abstract class ConstructorActivator : IActivator
    {
        private readonly IFactoryContainer _factoryContainer;
        private readonly ITypeSubstitution _typeSubstitution;
        private readonly IInstanceCache _instanceCache;

        protected ConstructorActivator(IFactoryContainer factoryContainer, ITypeSubstitution typeSubstitution, IInstanceCache instanceCache)
        {
            _factoryContainer = factoryContainer;
            _typeSubstitution = typeSubstitution;
            _instanceCache = instanceCache;
        }

        public object ActivateService(IInjectionContext context, ServiceIdentity identity)
        {
            identity = GetRealIdentity(identity);

            if (_instanceCache != null)
                return _instanceCache.Get(identity, () => CreateInstance(context, identity));

            return CreateInstance(context, identity);
        }

        private ServiceIdentity GetRealIdentity(ServiceIdentity identity)
        {
            var realType = _typeSubstitution.TryGetSubstitutedType(identity);
            if (realType != null)
                return new ServiceIdentity(realType, identity.ServiceName);

            return identity;
        }

        private object CreateInstance(IInjectionContext context, ServiceIdentity identity)
        {
            return _factoryContainer.CreateInstance(identity.ServiceType, context);
        }
    }
}