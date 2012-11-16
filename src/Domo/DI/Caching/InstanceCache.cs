using System;
using System.Collections.Generic;
using Domo.Extensions;

namespace Domo.DI.Caching
{
    public class InstanceCache : IInstanceCache
    {
        private readonly IDictionary<ServiceKey, object> _instances = new Dictionary<ServiceKey, object>();

        public void Add(Type type, string name, object instance)
        {
            var key = GetKey(type, name);

            _instances[key] = instance;
        }

        public object Get(Type type, string name, Func<object> factoryDelegate)
        {
            var key = GetKey(type, name);

            return _instances.TryGetValue(key, factoryDelegate);
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

        private static ServiceKey GetKey(Type type, string name)
        {
            return new ServiceKey(type, name);
        }
    }
}