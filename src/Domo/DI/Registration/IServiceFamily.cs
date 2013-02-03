using System;
using System.Collections.Generic;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public interface IServiceFamily
    {
        Type ServiceType { get; }

        void Add(IService service);
        ActivationDelegate GetActivationDelegate(ServiceIdentity identity);
        IEnumerable<ActivationDelegate> GetAllActivationDelegates();
    }
}