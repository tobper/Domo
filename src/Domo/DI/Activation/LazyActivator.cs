using System;

namespace Domo.DI.Activation
{
    public class LazyActivator : IActivator
    {
        private readonly IActivator _realActivator;

        public LazyActivator(IActivator realActivator)
        {
            _realActivator = realActivator;
        }

        public object ActivateService(IInjectionContext context, ServiceIdentity identity)
        {
            var realServiceType = identity.ServiceType.GenericTypeArguments[0];
            var realIdentity = new ServiceIdentity(realServiceType, identity.ServiceName);
            var realFactoryDelegate = GetFactoryDelegate(context, realIdentity);
            var lazy = Activator.CreateInstance(identity.ServiceType, realFactoryDelegate);

            return lazy;
        }

        private Func<object> GetFactoryDelegate(IInjectionContext context, ServiceIdentity realIdentity)
        {
            return () => _realActivator.ActivateService(context, realIdentity);
        }
    }
}