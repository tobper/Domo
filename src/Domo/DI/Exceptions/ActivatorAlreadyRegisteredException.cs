using System;

namespace Domo.DI
{
    public class ActivatorAlreadyRegisteredException : Exception
    {
        public ServiceIdentity Identity { get; private set; }

        public ActivatorAlreadyRegisteredException(ServiceIdentity identity)
            : base(CreateMessage(identity))
        {
            Identity = identity;
        }

        private static string CreateMessage(ServiceIdentity identity)
        {
            return (identity.ServiceName == null)
                ? string.Format("A service of type '{0}' has already been registered.", identity.ServiceType)
                : string.Format("A service of type '{0}' has already been registered with the name '{1}.", identity.ServiceType, identity.ServiceName);
        }
    }
}