using System;

namespace Domo.DI.Registration
{
    public interface IServiceRegistration
    {
        ServiceIdentity Identity { get; }

        IService GetService(IContainer container);
        IServiceRegistration WithName(string name);
        IServiceRegistration OnApply(Action<IContainer> action);
    }

    public interface IServiceRegistration<TService> : IServiceRegistration
    {
    }
}