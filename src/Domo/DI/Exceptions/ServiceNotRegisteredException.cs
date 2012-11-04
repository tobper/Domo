using System;

namespace Domo.DI
{
    public class ServiceNotRegisteredException : Exception
    {
        public Type ServiceType { get; private set; }

        public ServiceNotRegisteredException(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}