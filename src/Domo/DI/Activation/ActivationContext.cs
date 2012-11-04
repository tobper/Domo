using Domo.DI.Caching;

namespace Domo.DI.Activation
{
    // Todo: Rename to avoid mismatch with System.ActivationContext
    public class ActivationContext
    {
        public IServiceCache ScopedCache { get; private set; }
        public IServiceCache SingletonCache { get; private set; }

        public ActivationContext(IServiceCache singletonCache, IServiceCache scopedCache)
        {
            ScopedCache = scopedCache;
            SingletonCache = singletonCache;
        }
    }
}