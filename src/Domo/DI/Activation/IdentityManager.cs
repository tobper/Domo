using System;
using System.Collections.Generic;
using Domo.Extensions;

namespace Domo.DI.Activation
{
    public class IdentityManager : IIdentityManager
    {
        private readonly IDictionary<ServiceIdentity, ServiceIdentity> _realIdentities = new Dictionary<ServiceIdentity, ServiceIdentity>();

        public void AddAlias(ServiceIdentity alias, Type realServiceType)
        {
            var realIdentity = new ServiceIdentity(realServiceType, alias.ServiceName);

            _realIdentities.Add(alias, realIdentity);
        }

        public ServiceIdentity GetRealIdentity(ServiceIdentity identity)
        {
            return
                _realIdentities.TryGetValue(identity) ??
                identity;
        }
    }
}