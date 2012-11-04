using System;
using System.Collections.Generic;
using Domo.DI.Scanning;

namespace Domo.DI
{
    public interface IServiceContainer
    {
        void Register(Action<IServiceRegistration> registration);
        IEnumerable<object> Resolve(Type serviceType);
    }
}