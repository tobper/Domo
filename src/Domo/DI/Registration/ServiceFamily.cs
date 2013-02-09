using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Registration
{
    public class ServiceFamily : IServiceFamily, IEnumerable<IActivator>
    {
        private readonly IDictionary<ServiceIdentity, IActivator> _activators = new Dictionary<ServiceIdentity, IActivator>();

        public Type ServiceType { get; private set; }

        public ServiceFamily(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public void Add(IActivator activator)
        {
            if (activator == null)
                throw new ArgumentNullException("activator");

            if (activator.Identity.ServiceType != ServiceType)
                throw new ArgumentException("A activator can't be added with an identity that does not have the same ServiceType as the family.", "activator");

            if (_activators.ContainsKey(activator.Identity))
                throw new ActivatorAlreadyRegisteredException(activator.Identity);

            _activators.Add(activator.Identity, activator);
        }

        public IActivator GetActivator(ServiceIdentity identity)
        {
            // Asking for default instance and only one instance is registered ->
            // The registered instance will be returned regardless of name it is registered with.
            var activator = (IsDefaultIdentity(identity) && _activators.Count == 1)
                ? _activators.Values.First()
                : _activators.TryGetValue(identity);

            return activator;
        }

        public IActivator[] GetAllActivators()
        {
            return _activators.Values.ToArray();
        }

        private static bool IsDefaultIdentity(ServiceIdentity identity)
        {
            return identity.ServiceName == null;
        }

        IEnumerator<IActivator> IEnumerable<IActivator>.GetEnumerator()
        {
            return _activators.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _activators.Values.GetEnumerator();
        }
    }
}