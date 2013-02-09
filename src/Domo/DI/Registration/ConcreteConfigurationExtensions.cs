using System;
using System.Reflection;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class ConcreteConfigurationExtensions
    {
        public static IContainerConfiguration RegisterConcreteType<TService, TConcrete>(this IContainerConfiguration configuration)
            where TConcrete : TService
            where TService : class
        {
            configuration.
                Register<TService>().
                AsTransient().
                UsingConcreteType(typeof(TConcrete));

            return configuration;
        }

        public static IActivatorServiceRegistration UsingConcreteType(this IActivatorServiceRegistration registration, Type concreteType)
        {
            registration.
                OnApply(container =>
                {
                    container.
                        ServiceLocator.
                        Resolve<ITypeSubstitution>().
                        AddConcreteType(registration.Identity, concreteType);
                });

            return registration;
        }

        public static IActivatorServiceRegistration UsingConcreteType(this IActivatorServiceRegistration registration, TypeInfo concreteType)
        {
            return UsingConcreteType(registration, concreteType.AsType());
        }

        public static IActivatorServiceRegistration UsingConcreteType<TConcrete>(this IActivatorServiceRegistration registration)
        {
            var concreteType = typeof(TConcrete);

            UsingConcreteType(registration, concreteType);

            return registration;
        }

        public static IActivatorServiceRegistration<TService> UsingConcreteType<TService>(this IActivatorServiceRegistration<TService> registration, Type concreteType)
        {
            UsingConcreteType((IActivatorServiceRegistration)registration, concreteType);

            return registration;
        }

        public static IActivatorServiceRegistration<TService> UsingConcreteType<TService>(this IActivatorServiceRegistration<TService> registration, TypeInfo concreteType)
        {
            UsingConcreteType((IActivatorServiceRegistration)registration, concreteType);

            return registration;
        }
    }
}