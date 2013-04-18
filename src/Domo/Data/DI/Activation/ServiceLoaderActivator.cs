using Domo.DI;
using Domo.DI.Activation;

namespace Domo.Data.DI.Activation
{
    public class ServiceLoaderActivator<TData> : IActivator
        where TData : class
    {
        private readonly IServiceLoader<TData> _loader;

        public ServiceIdentity Identity { get; private set; }

        public ServiceLoaderActivator(IServiceLoader<TData> loader)
        {
            _loader = loader;
            Identity = new ServiceIdentity(typeof(TData));
        }

        public object ActivateService(IInjectionContext context)
        {
            return _loader.LoadService();
        }
    }
}