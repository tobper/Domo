using System;

namespace Domo.DI.Registration
{
    public static class InstanceActivatorRegistrationExtensions
    {
        public static IContainerConfiguration RegisterInstance(this IContainerConfiguration configuration, Type serviceType, object instance, string serviceName = null)
        {
            var identity = new ServiceIdentity(serviceType, serviceName);
            var service = new InstanceActivatorRegistration<object>(identity, instance);

            return configuration.Register(service);
        }

        public static IContainerConfiguration RegisterInstance<TService>(this IContainerConfiguration configuration, TService instance, string serviceName = null)
            where TService : class
        {
            var serviceType = typeof(TService);
            var identity = new ServiceIdentity(serviceType, serviceName);
            var service = new InstanceActivatorRegistration<TService>(identity, instance);

            return configuration.Register(service);
        }

        public static IInstanceActivatorRegistration UsingInstance(this IFluentRegistration fluentRegistration, object instance)
        {
            return new InstanceActivatorRegistration<object>(fluentRegistration, instance);
        }

        public static IInstanceActivatorRegistration<TService> UsingInstance<TService>(this IFluentRegistration<TService> fluentRegistration, TService instance)
            where TService : class
        {
            return new InstanceActivatorRegistration<TService>(fluentRegistration, instance);
        }
    }
}