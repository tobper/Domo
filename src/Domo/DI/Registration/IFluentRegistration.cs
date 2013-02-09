using System;

namespace Domo.DI.Registration
{
    public interface IFluentRegistration
    {
        Type ServiceType { get; }

        void ApplyRegistration(IContainerConfiguration configuration);
        void Using(IServiceRegistration serviceRegistration);
    }

    public interface IFluentRegistration<TService> : IFluentRegistration
    { }
}