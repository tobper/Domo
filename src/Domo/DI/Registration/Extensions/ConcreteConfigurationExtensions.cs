using System;
using System.Reflection;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class ConcreteConfigurationExtensions
    {
        public static IContainerConfiguration Register<TService, TConcrete>(this IContainerConfiguration configuration)
            where TConcrete : TService
        {
            configuration.
                Register<TService>().
                Using(typeof(TConcrete));

            return configuration;
        }

        public static TService Using<TService>(this TService service, Type concreteType)
            where TService : IActivatorServiceConfiguration
        {
            return Register(service, concreteType);
        }

        public static TService Using<TService>(this TService service, TypeInfo concreteType)
            where TService : IActivatorServiceConfiguration
        {
            return Register(service, concreteType.AsType());
        }

        private static TService Register<TService>(TService service, Type concreteType)
            where TService : IActivatorServiceConfiguration
        {
            return service.
                OnComplete<TService>(container =>
                {
                    container.
                        ServiceLocator.
                        Resolve<ITypeSubstitution>().
                        AddConcreteType(service.Identity, concreteType);
                });
        }
    }
}