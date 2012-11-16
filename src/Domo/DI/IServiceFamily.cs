using System;

namespace Domo.DI
{
    public interface IServiceFamily
    {
        Type ServiceType { get; }

        void AddActivator(Type activatorType, string serviceName);
        Type GetActivator(string serviceName);
        ServiceFamilyMember[] GetMembers();
    }
}