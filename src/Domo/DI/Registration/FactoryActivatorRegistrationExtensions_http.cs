using Domo.DI.Caching;

namespace Domo.DI.Registration
{
    public static partial class FactoryActivatorRegistrationExtensions
    {
        private static readonly IServiceScope HttpRequestScope = new HttpRequestScope();

        public static IFactoryActivatorRegistration InHttpRequestScope(this IFluentRegistration fluentRegistration)
        {
            return InScope(fluentRegistration, HttpRequestScope);
        }

        public static IFactoryActivatorRegistration<TService> InHttpRequestScope<TService>(this IFluentRegistration<TService> fluentRegistration)
            where TService : class
        {
            return InScope(fluentRegistration, HttpRequestScope);
        }
    }
}