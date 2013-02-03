using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public abstract class ServiceConfiguration : IServiceConfiguration
    {
        private readonly List<Action<IContainer>> _onCompleteHandlers = new List<Action<IContainer>>();

        protected ServiceConfiguration(Type serviceType)
        {
            Identity = new ServiceIdentity(serviceType);
        }

        protected ServiceConfiguration(ServiceIdentity identity)
        {
            Identity = identity;
        }

        public ServiceIdentity Identity { get; private set; }

        public abstract IService GetService(IContainer container);

        public IServiceConfiguration OnComplete(Action<IContainer> onComplete)
        {
            if (onComplete == null)
                throw new ArgumentNullException("onComplete");

            _onCompleteHandlers.Add(onComplete);

            return this;
        }

        public IServiceConfiguration WithName(string name)
        {
            Identity = new ServiceIdentity(Identity.ServiceType, name);
            return this;
        }

        protected void ProcessCompleteHandlers(IContainer container)
        {
            foreach (var handler in _onCompleteHandlers)
            {
                handler(container);
            }
        }
    }
}