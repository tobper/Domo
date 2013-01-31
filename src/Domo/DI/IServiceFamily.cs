using System;
using System.Collections.Generic;
using Domo.DI.Activation;

namespace Domo.DI
{
    public interface IServiceFamily
    {
        Type ServiceType { get; }

        void AddActivator(ServiceIdentity identity, IActivator activator);
        ActivationDelegate GetActivationDelegate(ServiceIdentity identity);
        IEnumerable<ActivationDelegate> GetAllActivationDelegates();
    }
}