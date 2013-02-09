using System;

namespace Domo.DI.Registration
{
    public interface IContainerConfiguration
    {
        IContainerConfiguration Register(IServiceRegistration serviceRegistration);
        IContainerConfiguration Register<TService>(Action<IFluentRegistration<TService>> action) where TService : class;
        IFluentRegistration Register(Type serviceType);
        IFluentRegistration<TService> Register<TService>() where TService : class;

        void ApplyRegistrations(IContainer container);
    }
}