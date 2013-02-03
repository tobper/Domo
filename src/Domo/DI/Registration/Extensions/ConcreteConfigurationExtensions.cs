using System;
using System.Reflection;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class ConcreteConfigurationExtensions
    {
        public static IContainerConfiguration Register<TService, TConcrete>(this IContainerConfiguration container)
            where TConcrete : TService
        {
            container.
                Register<TService>().
                Using(typeof(TConcrete));

            return container;
        }

        public static TConfiguration Using<TConfiguration>(this TConfiguration configuration, Type concreteType)
            where TConfiguration : IActivatorServiceConfiguration
        {
            return Register(configuration, concreteType);
        }

        public static TConfiguration Using<TConfiguration>(this TConfiguration configuration, TypeInfo concreteType)
            where TConfiguration : IActivatorServiceConfiguration
        {
            return Register(configuration, concreteType.AsType());
        }

        private static TConfiguration Register<TConfiguration>(TConfiguration configuration, Type concreteType)
            where TConfiguration : IActivatorServiceConfiguration
        {
            return configuration.
                OnComplete<TConfiguration>(container =>
                {
                    container.
                        ServiceLocator.
                        Resolve<ITypeSubstitution>().
                        AddConcreteType(configuration.Identity, concreteType);
                });
        }
    }
}