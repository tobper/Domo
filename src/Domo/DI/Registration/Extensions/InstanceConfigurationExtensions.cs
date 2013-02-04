using System;

namespace Domo.DI.Registration
{
    public static class InstanceConfigurationExtensions
    {
        public static IContainerConfiguration Register(this IContainerConfiguration configuration, Type serviceType, object instance, string serviceName = null)
        {
            var identity = new ServiceIdentity(serviceType, serviceName);
            var service = new InstanceServiceConfiguration(identity, instance);

            return configuration.Register(service);
        }

        public static IContainerConfiguration Register<TService>(this IContainerConfiguration configuration, TService instance, string serviceName = null)
            where TService : class
        {
            var serviceType = typeof(TService);
            var identity = new ServiceIdentity(serviceType, serviceName);
            var service = new InstanceServiceConfiguration(identity, instance);

            return configuration.Register(service);
        }
    }
}