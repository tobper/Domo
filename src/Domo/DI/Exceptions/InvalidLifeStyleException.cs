using System;

namespace Domo.DI
{
    public class InvalidLifeStyleException : Exception
    {
        public ServiceInfo Service { get; private set; }

        public InvalidLifeStyleException(ServiceInfo service)
            : base(CreateMessage(service))
        {
            Service = service;
        }

        private static string CreateMessage(ServiceInfo service)
        {
            return string.Format("Invalid life style ({0}) specified for service {1}.", service.LifeStyle, service.ServiceType.Name);
        }
    }
}