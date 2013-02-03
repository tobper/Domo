using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class ScopeConfigurationExtensions
    {
        public static TService InScope<TService>(this TService service, Scope scope)
            where TService : IActivatorServiceConfiguration
        {
            var activatorType = GetActivatorType(scope, service.Identity.ServiceType);

            return service.ActivatedBy<TService>(activatorType);
        }

        public static TService AsSingleton<TService>(this TService service)
            where TService : IActivatorServiceConfiguration
        {
            return InScope(service, Scope.Singleton);
        }

        public static TService AsTransient<TService>(this TService service)
            where TService : IActivatorServiceConfiguration
        {
            return InScope(service, Scope.Transient);
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