using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class LazyService : IService
    {
        private readonly Func<IInjectionContext, object> _delegate;

        public LazyService(ServiceIdentity identity, IService realService)
        {
            Identity = identity;
            _delegate = realService.GetTypedLazyDelegate();
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            return _delegate(context);
        }
    }
}