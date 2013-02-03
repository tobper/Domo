using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Registration
{
    public class ServiceFamily : IServiceFamily, IEnumerable<IService>
    {
        private readonly IDictionary<ServiceIdentity, IService> _services = new Dictionary<ServiceIdentity, IService>();

        public Type ServiceType { get; private set; }

        public ServiceFamily(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public void Add(IService service)
        {
            if (service == null)
                throw new ArgumentNullException("service");

            if (service.Identity.ServiceType != ServiceType)
                throw new ArgumentException("A service can not be added with an identity that does not have the same ServiceType as the family.", "service");

            if (_services.ContainsKey(service.Identity))
                throw new ServiceAlreadyRegisteredException(service.Identity);

            _services.Add(service.Identity, service);
        }

        public ActivationDelegate GetActivationDelegate(ServiceIdentity identity)
        {
            // Asking for default instance and only one instance is registered ->
            // The registered instance will be returned regardless of name it is registered with.
            var service = (IsDefaultIdentity(identity) && _services.Count == 1)
                ? _services.Values.First()
                : _services.TryGetValue(identity);

            return service.GetActivationDelegate();
        }

        private static bool IsDefaultIdentity(ServiceIdentity identity)
        {
            return identity.ServiceName == null;
        }

        public IEnumerable<ActivationDelegate> GetAllActivationDelegates()
        {
            foreach (var service in _services.Values)
            {
                yield return service.GetActivationDelegate();
            }
        }

        IEnumerator<IService> IEnumerable<IService>.GetEnumerator()
        {
            return _services.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _services.Values.GetEnumerator();
        }
    }
}