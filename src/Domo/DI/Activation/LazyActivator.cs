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

        public object ActivateInstance(ActivationContext activationContext, Type type, string name = null)
        {
            var serviceType = type.GenericTypeArguments[0];
            var factory = GetFactory(activationContext, name, serviceType);
            var lazy = System.Activator.CreateInstance(type, factory);

            return lazy;
        }

        private Func<object> GetFactory(ActivationContext activationContext, string name, Type serviceType)
        {
            return () => _realActivator.ActivateInstance(activationContext, serviceType, name);
        }
    }
}