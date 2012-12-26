using System;

namespace Domo.DI
{
    public interface IServiceFamily
    {
        Type ServiceType { get; }

        void AddActivator(ServiceIdentity identity, Type activatorType);
        Type GetActivator(ServiceIdentity identity);
        ServiceFamilyMember[] GetMembers();
    }
}