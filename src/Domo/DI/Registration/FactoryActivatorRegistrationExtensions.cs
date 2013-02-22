using Domo.DI.Caching;

namespace Domo.DI.Registration
{
    public static class FactoryActivatorRegistrationExtensions
    {
        private static readonly IServiceScope TransientScope = new TransientScope();
        private static readonly IServiceScope SingletonScope = new SingletonScope();

        public static IContainerConfiguration RegisterScoped<TService>(this IContainerConfiguration configuration, IServiceScope scope, string serviceName = null)
            where TService : class
        {
            configuration.
                Register<TService>(serviceName).
                InScope(scope);

            return configuration;
        }

        public static IContainerConfiguration RegisterScoped<TService, TConcrete>(this IContainerConfiguration configuration, IServiceScope scope, string serviceName = null)
            where TConcrete : TService
            where TService : class
        {
            configuration.
                Register<TService>(serviceName).
                InScope(scope).
                UsingConcreteType(typeof(TConcrete));

            return configuration;
        }

        public static IContainerConfiguration RegisterSingleton<TService>(this IContainerConfiguration configuration, string serviceName = null)
            where TService : class
        {
            return RegisterScoped<TService>(configuration, SingletonScope, serviceName);
        }

        public static IContainerConfiguration RegisterSingleton<TService, TConcrete>(this IContainerConfiguration configuration, string serviceName = null)
            where TConcrete : TService
            where TService : class
        {
            return RegisterScoped<TService, TConcrete>(configuration, SingletonScope, serviceName);
        }

        public static IContainerConfiguration RegisterTransient<TService>(this IContainerConfiguration configuration, string serviceName = null)
            where TService : class
        {
            return RegisterScoped<TService>(configuration, TransientScope, serviceName);
        }

        public static IContainerConfiguration RegisterTransient<TService, TConcrete>(this IContainerConfiguration configuration, string serviceName = null)
            where TConcrete : TService
            where TService : class
        {
            return RegisterScoped<TService, TConcrete>(configuration, TransientScope, serviceName);
        }

        public static IFactoryActivatorRegistration AsSingleton(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, SingletonScope);
        }

        public static IFactoryActivatorRegistration<TService> AsSingleton<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, SingletonScope);
        }

        public static IFactoryActivatorRegistration AsTransient(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, TransientScope);
        }

        public static IFactoryActivatorRegistration<TService> AsTransient<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, TransientScope);
        }

        public static IFactoryActivatorRegistration InScope(this IFluentRegistration fluentRegistration, IServiceScope serviceScope)
        {
            return new FactoryActivatorRegistration<object>(fluentRegistration, serviceScope);
        }

        public static IFactoryActivatorRegistration<TService> InScope<TService>(this IFluentRegistration<TService> fluentRegistration, IServiceScope serviceScope)
        {
            return new FactoryActivatorRegistration<TService>(fluentRegistration, serviceScope);
        }
    }
}