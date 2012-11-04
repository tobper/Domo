using System;
using System.Linq;

namespace Domo.DI
{
    public class ServiceLocator : IServiceLocator
    {
        public IServiceContainer Container { get; private set; }

        public ServiceLocator(IServiceContainer container)
        {
            Container = container;
        }

        public object Resolve(Type serviceType)
        {
            var service = TryResolve(serviceType);
            if (service == null)
                throw new ServiceNotRegisteredException(serviceType);

            return service;
        }

        public T Resolve<T>() where T : class
        {
            var serviceType = typeof(T);
            return (T)Resolve(serviceType);
        }

        public object[] ResolveAll(Type serviceType)
        {
            return Container.
                Resolve(serviceType).
                ToArray();
        }

        public T[] ResolveAll<T>() where T : class
        {
            var serviceType = typeof(T);

            return Container.
                Resolve(serviceType).
                Cast<T>().
                ToArray();
        }

        public object TryResolve(Type serviceType)
        {
            return Container.
                Resolve(serviceType).
                FirstOrDefault();
        }

        public T TryResolve<T>() where T : class
        {
            var serviceType = typeof(T);
            return (T)TryResolve(serviceType);
        }
    }
}