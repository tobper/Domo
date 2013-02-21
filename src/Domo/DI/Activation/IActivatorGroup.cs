using System;

namespace Domo.DI.Activation
{
    public interface IActivatorGroup
    {
        Type ServiceType { get; }

        void Add(IActivator activator);
        IActivator GetActivator(ServiceIdentity identity);
        IActivator[] GetAllActivators();
    }
}