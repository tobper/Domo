using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.DI.Construction;
using Domo.Extensions;

namespace Domo.DI.Caching
{
    public class DictionaryInstanceCache : IInstanceCache
    {
        private readonly IDictionary<ServiceIdentity, object> _instances = new Dictionary<ServiceIdentity, object>();

        public object Get(ServiceIdentity identity, IFactory factory, IInjectionContext context)
        {
            return _instances.TryGetValue(identity, () => factory.CreateInstance(context));
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