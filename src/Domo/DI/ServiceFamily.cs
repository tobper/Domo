using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Activation;
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

        public void AddActivator(ServiceIdentity identity, IActivator activator)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            if (identity == null)
                throw new ArgumentNullException("identity");

            if (identity.ServiceType != ServiceType)
                throw new ArgumentException("An activator can not be added with an identity that does not have the same ServiceType as the family.", "identity");

            if (_members.ContainsKey(identity))
                throw new ServiceAlreadyRegisteredException(identity);

            var member = new ServiceFamilyMember(identity, activator);

            _members.Add(identity, member);
        }

        public ActivationDelegate GetActivationDelegate(ServiceIdentity identity)
        {
            var member = _members.TryGetValue(identity);
            if (member == null)
            {
                if (_members.Count != 1)
                    return null;

                // Use the only member there is as a default member.
                member = _members.Values.Single();
            }

            return GetActivationDelegate(member);
        }

        public IEnumerable<ActivationDelegate> GetAllActivationDelegates()
        {
            return _members.Values.Select(GetActivationDelegate);
        }

        private static ActivationDelegate GetActivationDelegate(ServiceFamilyMember member)
        {
            return context => member.Activator.ActivateService(context, member.Identity);
        }
    }
}