using Domo.DI.Caching;
using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public abstract class ConstructorActivator : IActivator
    {
        private readonly IFactoryContainer _factoryContainer;
        private readonly IIdentityManager _identityManager;
        private readonly IInstanceCache _instanceCache;

        protected ConstructorActivator(IFactoryContainer factoryContainer, IIdentityManager identityManager, IInstanceCache instanceCache)
        {
            _factoryContainer = factoryContainer;
            _identityManager = identityManager;
            _instanceCache = instanceCache;
        }

        public object ActivateService(IInjectionContext context, ServiceIdentity identity)
        {
            identity = _identityManager.GetRealIdentity(identity);

            if (_instanceCache != null)
                return _instanceCache.Get(identity, () => CreateInstance(context, identity));

            return CreateInstance(context, identity);
        }

        private object CreateInstance(IInjectionContext context, ServiceIdentity identity)
        {
            return _factoryContainer.CreateInstance(identity.ServiceType, context);
        }
    }
}