using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class FuncService : IService
    {
        private readonly Func<IInjectionContext, object> _delegate;

        public FuncService(ServiceIdentity identity, IService realService)
        {
            Identity = identity;
            _delegate = realService.GetTypedFuncDelegate();
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            return _delegate(context);
        }
    }
}