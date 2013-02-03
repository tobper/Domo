using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class LazyService : IService
    {
        private readonly IService _realService;

        public LazyService(IService realService)
        {
            _realService = realService;
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            return CreateLazyInstance(context);
        }

        private object CreateLazyInstance(IInjectionContext context)
        {
            var lazyType = typeof(Lazy<>).MakeGenericType(Identity.ServiceType);
            var lazyDelegate = CreateLazyDelegate(context);

            return Activator.CreateInstance(lazyType, lazyDelegate);
        }

        private Func<object> CreateLazyDelegate(IInjectionContext context)
        {
            return () => _realService.GetInstance(context);
        }
    }
}