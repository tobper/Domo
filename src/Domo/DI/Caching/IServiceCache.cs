using System;
using Domo.DI.Activation;
using Domo.DI.Construction;

namespace Domo.DI.Caching
{
    public interface IServiceCache : IDisposable
    {
        object Get(ServiceIdentity identity, IFactory factory, IInjectionContext context);
    }
}