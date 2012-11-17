using System;

namespace Domo.DI
{
    public class ServiceNotRegisteredException : Exception
    {
        public Type ServiceType { get; private set; }
        public string ServiceName { get; private set; }

        public ServiceNotRegisteredException(Type serviceType)
            : base(CreateMessage(serviceType))
        {
            ServiceType = serviceType;
        }

        public ServiceNotRegisteredException(Type serviceType, string serviceName)
            : base(CreateMessage(serviceType, serviceName))
        {
            ServiceType = serviceType;
            ServiceName = serviceName;
        }

        private static string CreateMessage(Type serviceType)
        {
            return string.Format("No service of type '{0}' has been registered.", serviceType);
        }

        private static string CreateMessage(Type serviceType, string serviceName)
        {
            return (serviceName == null)
                ? CreateMessage(serviceType)
                : string.Format("No service of type '{0}' with the name '{1}' has been registered.", serviceType, serviceName);
        }
    }
}