using System;

namespace Domo.DI.Activation
{
    public class SingletonServiceActivator : IServiceActivator
    {
        private readonly Type _activationType;
        private readonly IInstanceFactory _instanceFactory;

        public SingletonServiceActivator(Type activationType, IInstanceFactory instanceFactory)
        {
            _activationType = activationType;
            _instanceFactory = instanceFactory;
        }

        public object ActivateInstance(ActivationContext activationContext)
        {
            return activationContext.SingletonCache.Get(_activationType, _instanceFactory, activationContext);
        }
    }
}