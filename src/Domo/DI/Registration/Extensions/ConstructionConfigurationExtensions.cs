using System;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public static class ConstructionConfigurationExtensions
    {
        public static TConfiguration CreatedWith<TConfiguration>(this TConfiguration configuration, IFactory factory)
            where TConfiguration : IActivatorServiceConfiguration
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            return configuration.
                OnComplete<TConfiguration>(container =>
                {
                    container.
                        ServiceLocator.
                        Resolve<IFactoryContainer>().
                        AddFactory(configuration.Identity, factory);
                });
        }
    }
}