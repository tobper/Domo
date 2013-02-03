using System;
using Domo.DI.Activation;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public static class DelegateFactoryConfigurationExtensions
    {
        public static TService CreatedWith<TService>(this TService service, Func<IInjectionContext, object> factoryDelegate)
            where TService : IActivatorServiceConfiguration
        {
            var factory = new DelegateFactory(factoryDelegate);

            return service.CreatedWith(factory);
        }

        public static TConfiguration CreatedWith<TConfiguration, TService>(this TConfiguration service, Func<IInjectionContext, TService> factoryDelegate)
            where TConfiguration : IActivatorServiceConfiguration<TService>
        {
            var factory = new DelegateFactory<TService>(factoryDelegate);

            return service.CreatedWith(factory);
        }
    }
}