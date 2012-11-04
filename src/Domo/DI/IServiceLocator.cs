using System;

namespace Domo.DI
{
    public interface IServiceLocator
    {
        IServiceContainer Container { get; }

        object Resolve(Type serviceType);
        T Resolve<T>() where T : class;
        object[] ResolveAll(Type serviceType);
        T[] ResolveAll<T>() where T : class;
        object TryResolve(Type serviceType);
        T TryResolve<T>() where T : class;
    }
}