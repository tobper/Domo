using System;
using System.Collections.Generic;
using System.Linq;
using Domo.Extensions;

namespace Domo.DI
{
    public class ServiceFamily : IServiceFamily
    {
        private const string DefaultServiceName = "Default";

        private readonly IDictionary<string, ServiceFamilyMember> _activators = new Dictionary<string, ServiceFamilyMember>(StringComparer.OrdinalIgnoreCase);

        public Type ServiceType { get; private set; }

        public ServiceFamily(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public void AddActivator(Type activatorType, string serviceName)
        {
            var member = new ServiceFamilyMember(activatorType, serviceName);
            var activatorKey = serviceName ?? DefaultServiceName;

            if (_activators.ContainsKey(activatorKey))
                throw new ServiceAlreadyRegisteredException(serviceName, ServiceType);

            _activators.Add(activatorKey, member);
        }

        public Type GetActivator(string serviceName)
        {
            var member = _activators.TryGetValue(serviceName ?? DefaultServiceName);
            if (member == null)
                return null;

            return member.ActivatorType;
        }

        public ServiceFamilyMember[] GetMembers()
        {
            return _activators.Values.ToArray();
        }
    }
}