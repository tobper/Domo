using System;
using System.Collections.Generic;
using Domo.DI.Caching;
using Domo.Extensions;

namespace Domo.DI.Redirection
{
    public class TypeRedirector : ITypeRedirector
    {
        private readonly IDictionary<ServiceKey, Type> _redirections = new Dictionary<ServiceKey, Type>();

        public void AddRedirection(Type serviceType, string serviceName, Type instanceType)
        {
            var key = new ServiceKey(serviceType, serviceName);

            _redirections.Add(key, instanceType);
        }

        public Type GetRedirection(Type serviceType, string serviceName)
        {
            var key = new ServiceKey(serviceType, serviceName);

            return _redirections.TryGetValue(key);
        }
    }
}