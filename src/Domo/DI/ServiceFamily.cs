using System;
using System.Collections.Generic;
using System.Linq;
using Domo.Extensions;

namespace Domo.DI
{
    public class ServiceFamily : IServiceFamily
    {
        private readonly IDictionary<ServiceIdentity, ServiceFamilyMember> _members = new Dictionary<ServiceIdentity, ServiceFamilyMember>();

        public Type ServiceType { get; private set; }

        public ServiceFamily(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public void AddActivator(ServiceIdentity identity, Type activatorType)
        {
            if (activatorType == null)
                throw new ArgumentNullException("activatorType");

            if (identity == null)
                throw new ArgumentNullException("identity");

            if (identity.ServiceType != ServiceType)
                throw new ArgumentException("An activator can not be added with an identity that does not have the same ServiceType as the family.", "identity");

            if (_members.ContainsKey(identity))
                throw new ServiceAlreadyRegisteredException(identity);

            var member = new ServiceFamilyMember(activatorType, identity);

            _members.Add(identity, member);
        }

        public Type GetActivator(ServiceIdentity identity)
        {
            var member = _members.TryGetValue(identity);
            if (member == null)
                return null;

            return member.ActivatorType;
        }

        public ServiceFamilyMember[] GetMembers()
        {
            return _members.Values.ToArray();
        }
    }
}