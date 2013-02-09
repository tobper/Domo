using System;
using Domo.DI.Registration;

namespace Domo.DI.Activation
{
    public class ArrayActivator : IActivator
    {
        private readonly Func<IInjectionContext, object> _delegate;

        public ArrayActivator(ServiceIdentity identity, Type itemServiceType, IActivator[] activators)
        {
            Identity = identity;
            _delegate = activators.GetTypedArrayDelegate(itemServiceType);
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            return _delegate(context);
        }
    }
}