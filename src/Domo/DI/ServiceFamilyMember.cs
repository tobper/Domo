using System;

namespace Domo.DI
{
    public class ServiceFamilyMember
    {
        public Type ActivatorType { get; private set; }
        public ServiceIdentity Identity { get; private set; }

        public ServiceFamilyMember(Type activatorType, ServiceIdentity identity)
        {
            ActivatorType = activatorType;
            Identity = identity;
        }
    }
}