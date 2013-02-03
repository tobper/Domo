using System;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public static class FactoryConfigurationExtensions
    {
        public static TService CreatedWith<TService>(this TService service, IFactory factory)
            where TService : IActivatorServiceConfiguration
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            return service.
                OnComplete<TService>(container =>
                {
                    container.
                        ServiceLocator.
                        Resolve<IFactoryContainer>().
                        AddFactory(service.Identity, factory);
                });
        }
    }
}