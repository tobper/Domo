using System;
using Domo.DI.Registration;

namespace Domo.DI.Activation
{
    public class FuncActivator : IActivator
    {
        private readonly Func<IInjectionContext, object> _delegate;

        public FuncActivator(ServiceIdentity identity, IActivator realActivator)
        {
            Identity = identity;
            _delegate = realActivator.GetTypedFuncDelegate();
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetService(IInjectionContext context)
        {
            return _delegate(context);
        }
    }
}