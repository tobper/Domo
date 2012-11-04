using System;

namespace Domo.DI.Activation
{
    public class ScopedServiceActivator : IServiceActivator
    {
        private readonly Type _activationType;
        private readonly IInstanceFactory _instanceFactory;

        public ScopedServiceActivator(Type activationType, IInstanceFactory instanceFactory)
        {
            _activationType = activationType;
            _instanceFactory = instanceFactory;
        }

        public object ActivateInstance(ActivationContext activationContext)
        {
            if (activationContext.ScopedCache == null)
                throw new ServiceScopeHasNotBeenDefinedException();

            return activationContext.ScopedCache.Get(_activationType, _instanceFactory, activationContext);
        }
    }
}