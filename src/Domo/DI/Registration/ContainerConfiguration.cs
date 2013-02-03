using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public class ContainerConfiguration : IContainerConfiguration
    {
        private readonly IContainer _container;
        private readonly Queue<IServiceConfiguration> _configurations = new Queue<IServiceConfiguration>();

        public ContainerConfiguration(IContainer container)
        {
            _container = container;
        }

        public IContainerConfiguration Register(IServiceConfiguration configuration)
        {
            if (configuration == null)
                throw new ArgumentNullException("configuration");

            _configurations.Enqueue(configuration);

            return this;
        }

        public void CompleteRegistration()
        {
            while (_configurations.Count > 0)
            {
                var service = _configurations.
                    Dequeue().
                    GetService(_container);

                _container.Register(service);
            }
        }
    }
}