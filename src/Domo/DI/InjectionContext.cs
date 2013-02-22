using System;
using System.Collections.Generic;
using Domo.DI.Activation;

namespace Domo.DI
{
    public class InjectionContext : IInjectionContext
    {
        public IContainer Container { get; private set; }
        public IDictionary<object, object> Data { get; private set; }

        public InjectionContext(IContainer container)
        {
            Container = container;
            Data = new Dictionary<object, object>();
        }

        public void Dispose()
        {
            foreach (var value in Data.Values)
            {
                var disposable = value as IDisposable;
                if (disposable != null)
                    disposable.Dispose();
            }
        }
    }
}