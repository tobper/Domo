using System;
using System.Collections.Generic;

namespace Domo.DI.Registration
{
    public class FluentRegistration<TService> :
        FluentRegistration,
        IFluentRegistration<TService>
    {
        public FluentRegistration(string serviceName, Queue<IActivatorConfiguration> activatorConfigurations)
            : base(new ServiceIdentity(typeof(TService), serviceName), activatorConfigurations)
        {
        }
    }

    public class FluentRegistration : IFluentRegistration
    {
        private readonly Queue<IActivatorConfiguration> _activatorConfigurations;
        private IActivatorConfiguration _activatorConfiguration;

        public ServiceIdentity Identity { get; private set; }

        public FluentRegistration(ServiceIdentity identity, Queue<IActivatorConfiguration> activatorConfigurations)
        {
            if (identity == null)
                throw new ArgumentNullException("identity");

            if (activatorConfigurations == null)
                throw new ArgumentNullException("activatorConfigurations");

            _activatorConfigurations = activatorConfigurations;

            Identity = identity;
        }

        public IActivatorConfiguration Configuration
        {
            get { return _activatorConfiguration; }
            set
            {
                if (_activatorConfiguration != null)
                    throw new InvalidOperationException("Todo");

                _activatorConfiguration = value;
                _activatorConfigurations.Enqueue(value);
            }
        }
    }
}