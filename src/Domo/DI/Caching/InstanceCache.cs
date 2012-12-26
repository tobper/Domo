using System;
using System.Collections.Generic;
using Domo.Extensions;

namespace Domo.DI.Caching
{
    public class InstanceCache : IInstanceCache
    {
        private readonly IDictionary<ServiceIdentity, object> _instances = new Dictionary<ServiceIdentity, object>();

        public void Add(ServiceIdentity identity, object instance)
        {
            _instances[identity] = instance;
        }

        public object Get(ServiceIdentity identity, Func<object> factoryDelegate)
        {
            return _instances.TryGetValue(identity, factoryDelegate);
        }

        public void Dispose()
        {
            foreach (var instance in _instances.Values)
            {
                var disposable = instance as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            _instances.Clear();
        }
    }
}