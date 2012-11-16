using System;

namespace Domo.DI.Activation
{
    public interface IActivator
    {
        object ActivateInstance(ActivationContext activationContext, Type type, string name = null);
    }
}