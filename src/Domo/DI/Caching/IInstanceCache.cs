using System;

namespace Domo.DI.Caching
{
    public interface IInstanceCache : IDisposable
    {
        void Add(ServiceIdentity identity, object instance);
        object Get(ServiceIdentity identity, Func<object> factoryDelegate);
    }
}