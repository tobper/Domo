using System;

namespace Domo.DI.Activation
{
    internal interface IActivatorContainer
    {
        IActivator GetActivator(Type activatorType);
    }
}