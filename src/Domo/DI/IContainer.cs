using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.DI
{
    public interface IContainer
    {
        IServiceLocator ServiceLocator { get; }

        void Configure(Action<IContainerConfiguration> configure = null, Action<IAssemblyScanner> scan = null);
        void Register(IActivator activator);
        object Resolve(ServiceIdentity identity);
        IEnumerable<object> ResolveAll(Type serviceType);

        IActivator GetActivator(ServiceIdentity identity);
        IActivator[] GetActivators(Type serviceType);
    }
}