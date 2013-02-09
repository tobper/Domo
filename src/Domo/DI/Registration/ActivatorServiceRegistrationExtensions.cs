using System;

namespace Domo.DI.Registration
{
    public static class ActivatorServiceRegistrationExtensions
    {
        public static IActivatorServiceRegistration UsingActivator(this IFluentRegistration fluentRegistration, Type activatorType)
        {
            return new ActivatorServiceRegistration(fluentRegistration, activatorType);
        }

        public static IActivatorServiceRegistration<TService> UsingActivator<TService>(this IFluentRegistration<TService> fluentRegistration, Type activatorType)
        {
            return new ActivatorServiceRegistration<TService>(fluentRegistration, activatorType);
        }

        public static IActivatorServiceRegistration UsingActivator<TActivator>(this IFluentRegistration fluentRegistration)
        {
            var activatorType = typeof(TActivator);

            return new ActivatorServiceRegistration(fluentRegistration, activatorType);
        }
    }
}