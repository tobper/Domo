using System;

namespace Domo.DI.Registration
{
    public interface IActivatorServiceRegistration : IServiceRegistration
    {
        Type ActivatorType { get; }
    }

    public interface IActivatorServiceRegistration<TService> :
        IActivatorServiceRegistration,
        IServiceRegistration<TService>
    {
    }
}