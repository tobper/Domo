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
            // Asking for default instance and only one instance is registered ->
            // The registered instance will be returned regardless of name it is registered with.
            var member = (identity.ServiceName == null && _members.Count == 1)
                ? _members.Values.First()
                : _members.TryGetValue(identity);

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