using System;
using Domo.DI.Activation;

namespace Domo.DI.Construction
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

    public class DelegateFactory<T> : DelegateFactory
    {
        public DelegateFactory(Func<T> factoryDelegate)
            : base(context => factoryDelegate())
        {
        }

        public DelegateFactory(Func<IInjectionContext, T> factoryDelegate)
            : base(context => factoryDelegate(context))
        {
        }
    }
}