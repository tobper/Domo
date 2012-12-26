using System;

namespace Domo.DI.Activation
{
    internal interface IActivatorContainer
    {
        IActivator this[Type activatorType] { get; }
    }
}