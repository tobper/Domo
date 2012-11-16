using System;
using Domo.DI.Activation;

namespace Domo.DI.Creation
{
    public class DelegateFactory : IFactory
    {
        private readonly Func<ActivationContext, object> _factoryDelegate;

        public DelegateFactory(Func<ActivationContext, object> factoryDelegate)
        {
            _factoryDelegate = factoryDelegate;
        }

        public object CreateInstance(ActivationContext activationContext)
        {
            return _factoryDelegate(activationContext);
        }
    }
}