using System;
using Domo.DI.Caching;
using Domo.DI.Creation;
using Domo.DI.Redirection;

namespace Domo.DI.Activation
{
    public abstract class FactoryActivator : IActivator
    {
        private readonly IFactoryManager _factoryManager;
        private readonly ITypeRedirector _typeRedirector;

        protected FactoryActivator(IFactoryManager factoryManager, ITypeRedirector typeRedirector)
        {
            _factoryManager = factoryManager;
            _typeRedirector = typeRedirector;
        }

        public abstract object ActivateInstance(ActivationContext activationContext, Type type, string name);

        protected object ActivateInstance(IInstanceCache instanceCache, ActivationContext activationContext, Type type, string name)
        {
            return instanceCache.Get(type, name, () => CreateInstance(activationContext, type, name));
        }

        protected object CreateInstance(ActivationContext activationContext, Type type, string name)
        {
            var instanceType = GetInstanceType(type, name);
            var factory = _factoryManager.GetFactory(instanceType);
            var instance = factory.CreateInstance(activationContext);

            return instance;
        }

        private Type GetInstanceType(Type type, string name)
        {
            var redirectedType = _typeRedirector.GetRedirection(type, name);
            var instanceType = redirectedType ?? type;

            return instanceType;
        }
    }
}