using Domo.DI.Caching;

namespace Domo.DI.Registration
{
    public static partial class FactoryActivatorRegistrationExtensions
    {
        private static readonly IServiceScope HttpRequestScope = new HttpRequestScope();
        private static readonly IServiceScope HttpSessionScope = new HttpSessionScope();

        public static IFactoryActivatorRegistration InHttpRequestScope(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, HttpRequestScope);
        }

        public static IFactoryActivatorRegistration<TService> InHttpRequestScope<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, HttpRequestScope);
        }

        public static IFactoryActivatorRegistration InHttpSessionScope(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, HttpSessionScope);
        }

        public static IFactoryActivatorRegistration<TService> InHttpSessionScope<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, HttpSessionScope);
        }
    }
}