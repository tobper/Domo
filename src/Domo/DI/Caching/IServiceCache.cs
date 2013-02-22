using System;

namespace Domo.DI.Caching
{
    public interface IServiceCache : IDisposable
    {
        object Get(ServiceIdentity identity, Func<object> factoryDelegate);
    }
}