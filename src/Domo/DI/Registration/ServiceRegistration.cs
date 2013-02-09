using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public abstract class ServiceRegistration : IServiceRegistration
    {
        private readonly List<Action<IContainer>> _onApplyHandlers = new List<Action<IContainer>>();

        protected ServiceRegistration(ServiceIdentity identity)
        {
            Identity = identity;
        }

        protected ServiceRegistration(IFluentRegistration fluentRegistration)
        {
            Identity = new ServiceIdentity(fluentRegistration.ServiceType);

            fluentRegistration.Using(this);
        }

        public ServiceIdentity Identity { get; private set; }

        public abstract IService GetService(IContainer container);

        public IServiceRegistration WithName(string name)
        {
            Identity = new ServiceIdentity(Identity.ServiceType, name);

            return this;
        }

        public IServiceRegistration OnApply(Action<IContainer> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            _onApplyHandlers.Add(action);

            return this;
        }

        protected void ProcessRegisterHandlers(IContainer container)
        {
            foreach (var handler in _onApplyHandlers)
            {
                handler(container);
            }
        }
    }

    public abstract class ServiceRegistration<TService> : ServiceRegistration, IServiceRegistration<TService>
    {
        protected ServiceRegistration(IFluentRegistration fluentRegistration) :
            base(fluentRegistration)
        {
        }
    }
}