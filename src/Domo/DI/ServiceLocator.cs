using System;
using System.Linq;

namespace Domo.DI
{
    public class ServiceLocator : IServiceLocator
    {
        public IContainer Container { get; private set; }

        public ServiceLocator(IContainer container)
        {
            Container = container;
        }

        public T[] ResolveAll<T>() where T : class
        {
            var serviceType = typeof(T);

            return Container.
                ResolveAll(serviceType).
                Cast<T>().
                ToArray();
        }

        public T Resolve<T>(string serviceName) where T : class
        {
            var serviceType = typeof(T);
            return (T)Resolve(serviceType, serviceName);
        }

        public T TryResolve<T>(string serviceName) where T : class
        {
            var serviceType = typeof(T);
            return (T)TryResolve(serviceType, serviceName);
        }

        public object[] ResolveAll(Type serviceType)
        {
            return Container.
                ResolveAll(serviceType).
                ToArray();
        }

        public object Resolve(Type serviceType, string serviceName)
        {
            var service = TryResolve(serviceType, serviceName);
            if (service == null)
                throw new ServiceNotRegisteredException(serviceType);

            return service;
        }

        public object TryResolve(Type serviceType, string serviceName)
        {
            // Todo: Container.Resolve throws in case of missing registration
            return Container.Resolve(serviceType, serviceName);
        }
    }
}