using Domo.DI.Activation;

namespace Domo.DI.Caching
{
    public interface IServiceScope
    {
        IServiceCache GetCache(IInjectionContext context);
    }
}