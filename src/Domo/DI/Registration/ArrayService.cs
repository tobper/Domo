using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class ArrayService : IService
    {
        private readonly Func<IInjectionContext, object> _delegate;

        public ArrayService(ServiceIdentity identity, Type itemServiceType, IService[] services)
        {
            Identity = identity;
            _delegate = services.GetTypedArrayDelegate(itemServiceType);
        }

        public ServiceIdentity Identity { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            return _delegate(context);
        }
    }
}