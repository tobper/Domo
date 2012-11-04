using System;

namespace Domo.DI.Scanning
{
    public interface IServiceRegistration
    {
        IServiceContainer Container { get; }

        IServiceRegistration RegisterService(ServiceInfo serviceInfo);
        IServiceRegistration RegisterSingleton<TService>(TService service) where TService : class;
        IServiceRegistration RegisterType<TService, TActivation>(LifeStyle lifeStyle = LifeStyle.Default);
        IServiceRegistration RegisterType(Type serviceType, Type activationType, LifeStyle lifeStyle = LifeStyle.Default);
        IServiceRegistration Scan(Action<IAssemblyScanner> scanner);
    }
}