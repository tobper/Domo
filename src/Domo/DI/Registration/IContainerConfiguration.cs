using System;

namespace Domo.DI.Registration
{
    public interface IContainerConfiguration
    {
        IContainerConfiguration Register(IActivatorConfiguration activatorConfiguration);
        IContainerConfiguration Register(Type serviceType, Action<IFluentRegistration> action);
        IContainerConfiguration Register<TService>(Action<IFluentRegistration<TService>> action) where TService : class;
        IFluentRegistration Register(ServiceIdentity identity);
        IFluentRegistration Register(Type serviceType, string serviceName = null);
        IFluentRegistration<TService> Register<TService>(string serviceName = null) where TService : class;

        IContainerConfiguration Scan(Action<IAssemblyScanner> scanner);

        void ApplyRegistrations(IContainer container);
    }
}