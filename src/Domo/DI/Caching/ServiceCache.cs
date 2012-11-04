using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Caching
{
    public class ServiceCache : IServiceCache
    {
        private readonly IDictionary<Type, object> _instances = new Dictionary<Type, object>();

        public void Add(Type type, object instance)
        {
            _instances[type] = instance;
        }

        public object Get(Type type, IInstanceFactory instanceFactory, ActivationContext activationContext)
        {
            return _instances.TryGetValue(type, () => instanceFactory.CreateInstance(activationContext));
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