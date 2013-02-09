using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class ActivatorServiceRegistration :
        ServiceRegistration,
        IActivatorServiceRegistration
    {
        public ActivatorServiceRegistration(ServiceIdentity identity, Type activatorType) :
            base(identity)
        {
            ActivatorType = activatorType;
        }

        public ActivatorServiceRegistration(IFluentRegistration fluentRegistration, Type activatorType) :
            base (fluentRegistration)
        {
            ActivatorType = activatorType;
        }

        public Type ActivatorType { get; private set; }

        public override IService GetService(IContainer container)
        {
            ProcessRegisterHandlers(container);

            var activators = container.ServiceLocator.Resolve<IActivatorContainer>();
            var activator = activators.GetActivator(ActivatorType);
            var service = new ActivatorService(Identity, activator);

            return service;
        }
    }

    public class ActivatorServiceRegistration<TService> :
        ActivatorServiceRegistration,
        IActivatorServiceRegistration<TService>
    {
        public ActivatorServiceRegistration(IFluentRegistration fluentRegistration, Type activatorType) :
            base(fluentRegistration, activatorType)
        {
        }
    }
}