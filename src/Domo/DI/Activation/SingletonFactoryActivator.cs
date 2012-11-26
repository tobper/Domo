using System;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Domo.DI.Redirection;

namespace Domo.DI.Activation
{
    public class SingletonFactoryActivator : FactoryActivator
    {
        private readonly IInstanceCache _instances;

        public SingletonFactoryActivator(IFactoryManager factoryManager, IInstanceCache singletonInstanceCache, ITypeRedirector typeRedirector)
            : base(factoryManager, typeRedirector)
        {
            _instances = singletonInstanceCache;
        }

        public override object ActivateInstance(ActivationContext activationContext, Type type, string name)
        {
            return ActivateInstance(_instances, activationContext, type, name);
        }
    }
}