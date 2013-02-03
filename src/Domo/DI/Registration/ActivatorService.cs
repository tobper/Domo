using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class ActivatorService : IService
    {
        public ServiceIdentity Identity { get; private set; }
        public IActivator Activator { get; private set; }

        public ActivatorService(ServiceIdentity identity, IActivator activator)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            if (activator == null)
                throw new ArgumentNullException("activator");

            Identity = identity;
            Activator = activator;
        }

        public ActivationDelegate GetActivationDelegate()
        {
            return context => Activator.ActivateService(context, Identity);
        }
    }
}