using System;

namespace Domo.DI.Registration
{
    public static class InstanceServiceRegistrationExtensions
    {
        public static IContainerConfiguration RegisterInstance(this IContainerConfiguration configuration, Type serviceType, object instance, string serviceName = null)
        {
            var identity = new ServiceIdentity(serviceType, serviceName);
            var service = new InstanceServiceRegistration(identity, instance);

            return configuration.Register(service);
        }

        public static IContainerConfiguration RegisterInstance<TService>(this IContainerConfiguration configuration, TService instance, string serviceName = null)
            where TService : class
        {
            var serviceType = typeof(TService);
            var identity = new ServiceIdentity(serviceType, serviceName);
            var service = new InstanceServiceRegistration(identity, instance);

            return configuration.Register(service);
        }

        public static IInstanceServiceRegistration Using(this IFluentRegistration fluentRegistration, object instance)
        {
            return new InstanceServiceRegistration(fluentRegistration, instance);
        }

        public static IInstanceServiceRegistration Using<TService>(this IFluentRegistration<TService> fluentRegistration, TService instance)
            where TService : class
        {
            return new InstanceServiceRegistration(fluentRegistration, instance);
        }
    }
}