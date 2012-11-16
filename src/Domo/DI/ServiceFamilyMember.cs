using System;

namespace Domo.DI
{
    public class ServiceFamilyMember
    {
        public Type ActivatorType { get; private set; }
        public string ServiceName { get; private set; }

        public ServiceFamilyMember(Type activatorType, string serviceName)
        {
            ActivatorType = activatorType;
            ServiceName = serviceName;
        }
    }
}