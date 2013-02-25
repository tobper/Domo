using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public class ContainerConfiguration : IContainerConfiguration
    {
        private readonly Queue<IActivatorConfiguration> _activatorConfigurations = new Queue<IActivatorConfiguration>();

        public IContainerConfiguration Register(IActivatorConfiguration activatorConfiguration)
        {
            if (activatorConfiguration == null)
                throw new ArgumentNullException("activatorConfiguration");

            _activatorConfigurations.Enqueue(activatorConfiguration);

            return this;
        }

        public IContainerConfiguration Register(Type serviceType, Action<IFluentRegistration> action)
        {
            if (action == null)
                throw new ArgumentNullException("action");

            var fluentRegistration = Register(serviceType);

            action(fluentRegistration);

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

        public IFluentRegistration Register(ServiceIdentity identity)
        {
            return new FluentRegistration(identity, _activatorConfigurations);
        }

        public IFluentRegistration Register(Type serviceType, string serviceName = null)
        {
            var identity = new ServiceIdentity(serviceType, serviceName);

            return new FluentRegistration(identity, _activatorConfigurations);
        }

        public IFluentRegistration<TService> Register<TService>(string serviceName = null)
            where TService : class
        {
            return new FluentRegistration<TService>(serviceName, _activatorConfigurations);
        }

        public IContainerConfiguration Scan(Action<IAssemblyScanner> scanner)
        {
            if (scanner == null)
                throw new ArgumentNullException("scanner");

            var scan = new AssemblyScanner(this);

            scanner(scan);

            return this;
        }

        public void ApplyRegistrations(IContainer container)
        {
            while (_activatorConfigurations.Count > 0)
            {
                var activator = _activatorConfigurations.
                    Dequeue().
                    GetActivator(container);

                container.Register(activator);
            }
        }
    }
}