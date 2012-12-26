using System;
using Domo.DI.Activation;

namespace Domo.DI.Creation
{
    public class DelegateFactory : IFactory
    {
        private readonly Func<IInjectionContext, object> _factoryDelegate;

        public DelegateFactory(Func<IInjectionContext, object> factoryDelegate)
        {
            _factoryDelegate = factoryDelegate;
        }

        public object CreateInstance(IInjectionContext context)
        {
            return _factoryDelegate(context);
        }
    }

    public class DelegateFactory<T> : IFactory
    {
        private readonly Func<IInjectionContext, T> _factoryDelegate;

        public DelegateFactory(Func<IInjectionContext, T> factoryDelegate)
        {
            _factoryDelegate = factoryDelegate;
        }

        public object CreateInstance(IInjectionContext context)
        {
            return _factoryDelegate(context);
        }
    }
}