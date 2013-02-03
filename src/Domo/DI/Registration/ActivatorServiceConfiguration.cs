using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class ActivatorServiceConfiguration : ServiceConfiguration, IActivatorServiceConfiguration
    {
        public ActivatorServiceConfiguration(Type serviceType) :
            base(serviceType)
        {
        }

        public ActivatorServiceConfiguration(ServiceIdentity identity) :
            base(identity)
        {
        }

        public Type ActivatorType { get; private set; }

        public IActivatorServiceConfiguration ActivatedBy(Type activatorType)
        {
            if (activatorType == null)
                throw new ArgumentNullException("activatorType");

            if (ActivatorType != null)
                throw new ActivatorHasAlreadyBeenSetException(Identity, ActivatorType, activatorType);

            ActivatorType = activatorType;

            return this;
        }

        public override IService GetService(IContainer container)
        {
            if (ActivatorType == null)
                this.AsTransient();

            ProcessCompleteHandlers(container);

            var activators = container.ServiceLocator.Resolve<IActivatorContainer>();
            var activator = activators.GetActivator(ActivatorType);
            var service = new ActivatorService(Identity, activator);

            return service;
        }
    }

    public class ActivatorServiceConfiguration<TService> :
        ActivatorServiceConfiguration,
        IActivatorServiceConfiguration<TService>
    {
        public ActivatorServiceConfiguration() :
            base(typeof(TService))
        {
        }
    }
}