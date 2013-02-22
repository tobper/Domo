using System;
using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.DI.Construction;
using Domo.Extensions;

namespace Domo.DI.Caching
{
    public class DictionaryServiceCache : IServiceCache
    {
        private readonly IDictionary<ServiceIdentity, object> _services = new Dictionary<ServiceIdentity, object>();

        public object Get(ServiceIdentity identity, IFactory factory, IInjectionContext context)
        {
            return _services.TryGetValue(identity, () => factory.CreateService(context));
        }

        public void Dispose()
        {
            foreach (var service in _services.Values)
            {
                var disposable = service as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }

            _services.Clear();
        }
    }
}