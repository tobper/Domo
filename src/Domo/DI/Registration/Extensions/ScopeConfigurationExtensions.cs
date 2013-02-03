using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class ScopeConfigurationExtensions
    {
        public static TConfiguration InScope<TConfiguration>(this TConfiguration configuration, Scope scope)
            where TConfiguration : IActivatorServiceConfiguration
        {
            var activatorType = GetActivatorType(scope, configuration.Identity.ServiceType);

            return configuration.ActivatedBy<TConfiguration>(activatorType);
        }

        public static TConfiguration AsSingleton<TConfiguration>(this TConfiguration configuration)
            where TConfiguration : IActivatorServiceConfiguration
        {
            return InScope(configuration, Scope.Singleton);
        }

        public static TConfiguration AsTransient<TConfiguration>(this TConfiguration configuration)
            where TConfiguration : IActivatorServiceConfiguration
        {
            return InScope(configuration, Scope.Transient);
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