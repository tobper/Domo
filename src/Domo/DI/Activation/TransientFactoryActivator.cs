using System;
using Domo.DI.Creation;
using Domo.DI.Redirection;

namespace Domo.DI.Activation
{
    public class TransientFactoryActivator : FactoryActivator
    {
        public TransientFactoryActivator(IFactoryManager factoryManager, ITypeRedirector typeRedirector)
            : base(factoryManager, typeRedirector)
        {
        }

        public override object ActivateInstance(ActivationContext activationContext, Type type, string name)
        {
            return CreateInstance(activationContext, type, name);
        }
    }
}