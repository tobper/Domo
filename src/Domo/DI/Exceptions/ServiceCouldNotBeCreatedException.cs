using System;

namespace Domo.DI
{
    public class ServiceCouldNotBeCreatedException : Exception
    {
        public Type ServiceType { get; private set; }

        public ServiceCouldNotBeCreatedException(Type serviceType)
        {
            ServiceType = serviceType;
        }
    }
}