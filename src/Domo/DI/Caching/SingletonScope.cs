using Domo.DI.Activation;

namespace Domo.DI.Caching
{
    public class SingletonScope : IServiceScope
    {
        public IServiceCache GetCache(IInjectionContext context)
        {
            return context.Container.ServiceLocator.Resolve<IServiceCache>();
        }
    }
}