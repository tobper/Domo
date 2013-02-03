using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.DI
{
    public interface IContainer
    {
        IServiceLocator ServiceLocator { get; }

        void Configure(Action<IContainerConfiguration> registration = null, Action<IAssemblyScanner> scanner = null);
        void Register(IService service);
        object Resolve(ServiceIdentity identity);
        IEnumerable<object> ResolveAll(Type serviceType);

        ActivationDelegate GetActivationDelegate(ServiceIdentity identity);
    }
}