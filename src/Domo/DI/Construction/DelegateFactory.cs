using System;
using Domo.DI.Activation;

namespace Domo.DI.Construction
{
    public class DelegateFactory : IFactory
    {
        private readonly Func<IInjectionContext, object> _factoryDelegate;

        public DelegateFactory(Func<object> factoryDelegate)
        {
            _factoryDelegate = context => factoryDelegate();
        }

        public DelegateFactory(Func<IInjectionContext, object> factoryDelegate)
        {
            _factoryDelegate = factoryDelegate;
        }

        public object CreateService(IInjectionContext context)
        {
            return _factoryDelegate(context);
        }
    }

    public class DelegateFactory<TService> : IFactory
    {
        private readonly Func<IInjectionContext, TService> _factoryDelegate;

        public DelegateFactory(Func<TService> factoryDelegate)
        {
            _factoryDelegate = context => factoryDelegate();
        }

        public DelegateFactory(Func<IInjectionContext, TService> factoryDelegate)
        {
            _factoryDelegate = factoryDelegate;
        }

        public object CreateService(IInjectionContext context)
        {
            return _factoryDelegate(context);
        }
    }
}