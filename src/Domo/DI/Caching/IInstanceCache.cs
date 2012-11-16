using System;

namespace Domo.DI.Caching
{
    public interface IInstanceCache : IDisposable
    {
        void Add(Type type, string name, object instance);
        object Get(Type type, string name, Func<object> factoryDelegate);
    }
}