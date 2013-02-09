using System;

namespace Domo.DI.Registration
{
    public class FluentRegistration<TService> :
        FluentRegistration,
        IFluentRegistration<TService>
    {
        public FluentRegistration(string serviceName)
            : base(typeof(TService), serviceName)
        {
        }
    }

    public class FluentRegistration :
        IFluentRegistration,
        IFluentConfiguration
    {
        public ServiceIdentity Identity { get; private set; }
        public IActivatorConfiguration ActivatorConfiguration { get; private set; }

        public FluentRegistration(Type serviceType, string serviceName)
        {
            if (serviceType == null)
                throw new ArgumentNullException("serviceType");

            Identity = new ServiceIdentity(serviceType, serviceName);
        }

        public FluentRegistration(ServiceIdentity identity)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            Identity = identity;
        }

        public IActivatorConfiguration GetActivatorConfiguration()
        {
            return ActivatorConfiguration;
        }

        public void Using(IActivatorConfiguration activatorConfiguration)
        {
            ActivatorConfiguration = activatorConfiguration;
        }
    }
}