using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class FactoryActivatorRegistrationExtensions
    {
        public static IContainerConfiguration RegisterScoped<TService>(this IContainerConfiguration configuration, ActivationScope activationScope, string serviceName = null)
            where TService : class
        {
            configuration.
                Register<TService>(serviceName).
                InScope(activationScope);

            return configuration;
        }

        public static IContainerConfiguration RegisterScoped<TService, TConcrete>(this IContainerConfiguration configuration, ActivationScope activationScope, string serviceName = null)
            where TConcrete : TService
            where TService : class
        {
            configuration.
                Register<TService>(serviceName).
                InScope(activationScope).
                UsingConcreteType(typeof(TConcrete));

            return configuration;
        }

        public static IContainerConfiguration RegisterSingleton<TService>(this IContainerConfiguration configuration, string serviceName = null)
            where TService : class
        {
            return RegisterScoped<TService>(configuration, ActivationScope.Singleton, serviceName);
        }

        public static IContainerConfiguration RegisterSingleton<TService, TConcrete>(this IContainerConfiguration configuration, string serviceName = null)
            where TConcrete : TService
            where TService : class
        {
            return RegisterScoped<TService, TConcrete>(configuration, ActivationScope.Singleton, serviceName);
        }

        public static IContainerConfiguration RegisterTransient<TService>(this IContainerConfiguration configuration, string serviceName = null)
            where TService : class
        {
            return RegisterScoped<TService>(configuration, ActivationScope.Transient, serviceName);
        }

        public static IContainerConfiguration RegisterTransient<TService, TConcrete>(this IContainerConfiguration configuration, string serviceName = null)
            where TConcrete : TService
            where TService : class
        {
            return RegisterScoped<TService, TConcrete>(configuration, ActivationScope.Transient, serviceName);
        }

        public static IFactoryActivatorRegistration AsTransient(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, ActivationScope.Transient);
        }

        public static IFactoryActivatorRegistration<TService> AsTransient<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, ActivationScope.Transient);
        }

        public static IFactoryActivatorRegistration AsSingleton(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, ActivationScope.Singleton);
        }

        public static IFactoryActivatorRegistration<TService> AsSingleton<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, ActivationScope.Singleton);
        }

        public static IFactoryActivatorRegistration InScope(this IFluentRegistration fluentRegistration, ActivationScope activationScope)
        {
            return new FactoryActivatorRegistration<object>(fluentRegistration, activationScope);
        }

        public static IFactoryActivatorRegistration<TService> InScope<TService>(this IFluentRegistration<TService> fluentRegistration, ActivationScope activationScope)
            where TService : class
        {
            return new FactoryActivatorRegistration<TService>(fluentRegistration, activationScope);
        }
    }
}