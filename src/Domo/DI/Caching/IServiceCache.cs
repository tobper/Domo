using System;
using Domo.DI.Activation;

namespace Domo.DI.Caching
{
    public interface IServiceCache : IDisposable
    {
        void Add(Type type, object instance);
        object Get(Type type, IInstanceFactory instanceFactory, ActivationContext activationContext);
    }
}