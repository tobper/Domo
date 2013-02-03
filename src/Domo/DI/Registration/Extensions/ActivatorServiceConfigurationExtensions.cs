using System;

namespace Domo.DI.Registration
{
    public static class ActivatorServiceConfigurationExtensions
    {
        public static IActivatorServiceConfiguration Register(this IContainerConfiguration configuration, ServiceIdentity identity)
        {
            var service = new ActivatorServiceConfiguration(identity);
            configuration.Register(service);
            return service;
        }

        public static IActivatorServiceConfiguration Register(this IContainerConfiguration configuration, Type serviceType)
        {
            var service = new ActivatorServiceConfiguration(serviceType);
            configuration.Register(service);
            return service;
        }

        public static IActivatorServiceConfiguration<TService> Register<TService>(this IContainerConfiguration configuration)
        {
            var service = new ActivatorServiceConfiguration<TService>();
            configuration.Register(service);
            return service;
        }

        public static IContainerConfiguration Register(this IContainerConfiguration configuration, ServiceIdentity identity, Action<IActivatorServiceConfiguration> register)
        {
            var service = configuration.Register(identity);
            register(service);
            return configuration;
        }

        public static IContainerConfiguration Register(this IContainerConfiguration configuration, Type serviceType, Action<IActivatorServiceConfiguration> register)
        {
            var service = configuration.Register(serviceType);
            register(service);
            return configuration;
        }

        public static IContainerConfiguration Register<TService>(this IContainerConfiguration configuration, Action<IActivatorServiceConfiguration<TService>> register)
        {
            var service = configuration.Register<TService>();
            register(service);
            return configuration;
        }

        public static TConfiguration ActivatedBy<TConfiguration>(this TConfiguration configuration, Type activatorType)
            where TConfiguration : IActivatorServiceConfiguration
        {
            configuration.ActivatedBy(activatorType);
            return configuration;
        }

        public static TConfiguration OnComplete<TConfiguration>(this TConfiguration configuration, Action<IContainer> onComplete)
            where TConfiguration : IActivatorServiceConfiguration
        {
            configuration.OnComplete(onComplete);
            return configuration;
        }
    }
}