using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public class ContainerConfiguration : IContainerConfiguration
    {
        private readonly IContainer _container;
        private readonly Queue<IServiceConfiguration> _serviceConfigurations = new Queue<IServiceConfiguration>();

        public ContainerConfiguration(IContainer container)
        {
            _container = container;
        }

        public IContainerConfiguration Register(IServiceConfiguration serviceConfiguration)
        {
            if (serviceConfiguration == null)
                throw new ArgumentNullException("serviceConfiguration");

            _serviceConfigurations.Enqueue(serviceConfiguration);

            return this;
        }

        public void CompleteRegistration()
        {
            while (_serviceConfigurations.Count > 0)
            {
                var service = _serviceConfigurations.
                    Dequeue().
                    GetService(_container);

                _container.Register(service);
            }
        }
    }
}