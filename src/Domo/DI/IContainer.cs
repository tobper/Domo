using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.DI.Creation;
using Domo.DI.Registration;

namespace Domo.DI
{
    public interface IContainer
    {
        void Register(Action<ITypeRegistration> registration);
        void Register(Type serviceType, string serviceName, Type activatorType);

        object Resolve(Type serviceType, string serviceName);
        IEnumerable<object> ResolveAll(Type serviceType);

        IActivator GetActivator(Type serviceType, string serviceName);
        IFactory GetFactory(Type serviceType);
        void Scan(Action<IAssemblyScanner> scanner);
    }
}