using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class ScopeServiceRegistrationExtensions
    {
        public static IActivatorServiceRegistration AsTransient(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, Scope.Transient);
        }

        public static IActivatorServiceRegistration<TService> AsTransient<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, Scope.Transient);
        }

        public static IActivatorServiceRegistration AsSingleton(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, Scope.Singleton);
        }

        public static IActivatorServiceRegistration<TService> AsSingleton<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, Scope.Singleton);
        }

        public static IActivatorServiceRegistration InScope(this IFluentRegistration fluentRegistration, Scope scope)
        {
            var activatorType = GetActivatorType(scope, fluentRegistration.ServiceType);

            return fluentRegistration.UsingActivator(activatorType);
        }

        public static IActivatorServiceRegistration<TService> InScope<TService>(this IFluentRegistration<TService> fluentRegistration, Scope scope)
            where TService : class
        {
            var activatorType = GetActivatorType(scope, fluentRegistration.ServiceType);

            return fluentRegistration.UsingActivator(activatorType);
        }

        private static Type GetActivatorType(Scope scope, Type serviceType)
        {
            switch (scope)
            {
                case Scope.Singleton:
                    return typeof(SingletonActivator);

                case Scope.Transient:
                    return typeof(TransientActivator);

                default:
                    throw new InvalidScopeException(scope, serviceType);
            }
        }
    }
}