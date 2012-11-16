using System;

namespace Domo.DI
{
    public interface IServiceLocator
    {
        IContainer Container { get; }

        T[] ResolveAll<T>() where T : class;
        T Resolve<T>(string serviceName = null) where T : class;
        T TryResolve<T>(string serviceName = null) where T : class;

        object[] ResolveAll(Type serviceType);
        object Resolve(Type serviceType, string serviceName = null);
        object TryResolve(Type serviceType, string serviceName = null);
    }
}