using System;

namespace Domo.DI
{
    public class ActivatorConfigurationAlreadyRegisteredException : Exception
    {
        public ServiceIdentity Identity { get; private set; }

        public ActivatorConfigurationAlreadyRegisteredException(ServiceIdentity identity)
            : base(CreateMessage(identity))
        {
            Identity = identity;
        }

        private static string CreateMessage(ServiceIdentity identity)
        {
            return (identity.ServiceName == null)
                ? string.Format("An activator configuration has already been set for the registration of a service of type '{0}'.", identity.ServiceType)
                : string.Format("An activator configuration has already been set for the registration of a service of type '{0}' with the name '{1}.", identity.ServiceType, identity.ServiceName);
        }
    }
}