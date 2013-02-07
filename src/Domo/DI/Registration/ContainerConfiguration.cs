using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public class ContainerConfiguration : IContainerConfiguration
    {
        private readonly Queue<IServiceConfiguration> _serviceConfigurations = new Queue<IServiceConfiguration>();

        public IContainerConfiguration Register(IServiceConfiguration serviceConfiguration)
        {
            if (serviceConfiguration == null)
                throw new ArgumentNullException("serviceConfiguration");

            _serviceConfigurations.Enqueue(serviceConfiguration);

            return this;
        }

        public void ApplyRegistration(IContainer container)
        {
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