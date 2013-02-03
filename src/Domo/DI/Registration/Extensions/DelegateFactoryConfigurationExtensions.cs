using System;
using Domo.DI.Activation;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public static class DelegateFactoryConfigurationExtensions
    {
        public static TConfiguration CreatedWith<TConfiguration>(this TConfiguration configuration, Func<IInjectionContext, object> factoryDelegate)
            where TConfiguration : IActivatorServiceConfiguration
        {
            var factory = new DelegateFactory(factoryDelegate);

            return configuration.CreatedWith(factory);
        }

        public static TConfiguration CreatedWith<TConfiguration, TService>(this TConfiguration configuration, Func<IInjectionContext, TService> factoryDelegate)
            where TConfiguration : IActivatorServiceConfiguration<TService>
        {
            var factory = new DelegateFactory<TService>(factoryDelegate);

            return configuration.CreatedWith(factory);
        }
    }
}