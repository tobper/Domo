using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public interface IServiceFamily
    {
        Type ServiceType { get; }

        void Add(IActivator activator);
        IActivator GetActivator(ServiceIdentity identity);
        IActivator[] GetAllActivators();
    }
}