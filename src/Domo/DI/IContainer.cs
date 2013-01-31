using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.DI
{
    public interface IContainer
    {
        IServiceLocator ServiceLocator { get; }

        void Register(Action<ITypeRegistration> registration);
        void Register(ServiceIdentity identity, Type activatorType);

        object Resolve(ServiceIdentity identity);
        IEnumerable<object> ResolveAll(Type serviceType);

        ActivationDelegate GetActivationDelegate(ServiceIdentity identity);
        void Scan(Action<IAssemblyScanner> scanner);
    }
}