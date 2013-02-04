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
            var instances = Container.ResolveAll(serviceType);
            if (instances == null)
                throw new ServiceNotRegisteredException(serviceType);

            return instances.
                Cast<T>().
                ToArray();
        }

        public T Resolve<T>(string serviceName) where T : class
        {
            var serviceType = typeof(T);
            var instance = (T)Resolve(serviceType, serviceName);

            return instance;
        }

        public T TryResolve<T>(string serviceName) where T : class
        {
            var serviceType = typeof(T);
            var instance = (T)TryResolve(serviceType, serviceName);

            return instance;
        }

        public object[] ResolveAll(Type serviceType)
        {
            var instances = Container.ResolveAll(serviceType);
            if (instances == null)
                throw new ServiceNotRegisteredException(serviceType);
            
            return instances.ToArray();
        }

        public object Resolve(Type serviceType, string serviceName)
        {
            var instance = TryResolve(serviceType, serviceName);
            if (instance == null)
                throw new ServiceNotRegisteredException(serviceType, serviceName);

            return instance;
        }

        public object TryResolve(Type serviceType, string serviceName)
        {
            var identity = new ServiceIdentity(serviceType, serviceName);
            var instance = Container.Resolve(identity);

            return instance;
        }
    }
}