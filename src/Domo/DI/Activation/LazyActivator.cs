using System;
using Domo.DI.Registration;

namespace Domo.DI.Activation
{
    public class LazyActivator : IActivator
    {
        private readonly Func<IInjectionContext, object> _delegate;

        public LazyActivator(ServiceIdentity identity, IActivator realActivator)
        {
            Identity = identity;
            _delegate = realActivator.GetTypedLazyDelegate();
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            return _delegate(context);
        }
    }
}