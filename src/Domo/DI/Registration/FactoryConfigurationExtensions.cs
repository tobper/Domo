using System;
using Domo.DI.Activation;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public static class FactoryConfigurationExtensions
    {
        public static TRegistration UsingFactory<TRegistration>(this TRegistration registration, Func<IInjectionContext, object> factoryDelegate)
            where TRegistration : IActivatorServiceRegistration
        {
            var delegateFactory = new DelegateFactory(factoryDelegate);

            return UsingFactory(registration, delegateFactory);
        }

        public static TRegistration UsingFactory<TRegistration>(this TRegistration registration, IFactory factory)
            where TRegistration : IActivatorServiceRegistration
        {
            if (factory == null)
                throw new ArgumentNullException("factory");

            registration.
                OnApply(container =>
                {
                    container.
                        ServiceLocator.
                        Resolve<IFactoryContainer>().
                        AddFactory(registration.Identity, factory);
                });

            return registration;
        }
    }
}