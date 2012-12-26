using System;

namespace Domo.DI
{
    public class ServiceAlreadyRegisteredException : Exception
    {
        public string ServiceName { get; private set; }
        public Type ServiceType { get; private set; }

        public ServiceAlreadyRegisteredException(ServiceIdentity identity)
            : this(identity.ServiceName, identity.ServiceType)
        {
        }

        public ServiceAlreadyRegisteredException(string serviceName, Type serviceType)
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