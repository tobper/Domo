using System;

namespace Domo.DI
{
    public class ActivatorAlreadyRegisteredException : Exception
    {
        public string ServiceName { get; private set; }
        public Type ServiceType { get; private set; }

        public ActivatorAlreadyRegisteredException(ServiceIdentity identity)
            : this(identity.ServiceName, identity.ServiceType)
        {
        }

        public ActivatorAlreadyRegisteredException(string serviceName, Type serviceType)
            : base(CreateMessage(serviceName, serviceType))
        {
            ServiceName = serviceName;
            ServiceType = serviceType;
        }

        private static string CreateMessage(string serviceName, Type serviceType)
        {
            return (serviceName == null)
                ? string.Format("A service of type '{0}' has already been registered.", serviceType)
                : string.Format("A service of type '{0}' has already been registered with the name '{1}.", serviceType, serviceName);
        }
    }
}