using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public class ContainerConfiguration : IContainerConfiguration
    {
        private readonly Queue<IServiceRegistration> _serviceConfigurations = new Queue<IServiceRegistration>();
        private readonly Queue<IFluentRegistration> _fluentRegistrations = new Queue<IFluentRegistration>();

        public IContainerConfiguration Register(IServiceRegistration serviceRegistration)
        {
            if (serviceRegistration == null)
                throw new ArgumentNullException("serviceRegistration");

            _serviceConfigurations.Enqueue(serviceRegistration);

            return this;
        }

        public IContainerConfiguration Register<TService>(Action<IFluentRegistration<TService>> action)
            where TService : class
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var fluentRegistration = Register<TService>();
            action(fluentRegistration);

            return this;
        }

        public IFluentRegistration Register(Type serviceType)
        {
            var registration = new FluentRegistration(serviceType);

            _fluentRegistrations.Enqueue(registration);

            return registration;
        }

        public IFluentRegistration<TService> Register<TService>()
            where TService : class
        {
            var registration = new FluentRegistration<TService>();

            _fluentRegistrations.Enqueue(registration);

            return registration;
        }

        public void ApplyRegistrations(IContainer container)
        {
            while (_fluentRegistrations.Count > 0)
            {
                _fluentRegistrations.
                    Dequeue().
                    ApplyRegistration(this);
            }

            while (_serviceConfigurations.Count > 0)
            {
                var service = _serviceConfigurations.
                    Dequeue().
                    GetService(container);

                container.Register(service);
            }
        }
    }
}