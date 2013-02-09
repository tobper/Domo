using System;

namespace Domo.DI.Registration
{
    public class FluentRegistration : IFluentRegistration
    {
        private IServiceRegistration _serviceRegistration;

        public FluentRegistration(Type serviceType)
        {
            ServiceType = serviceType;
        }

        public Type ServiceType { get; private set; }

        public void ApplyRegistration(IContainerConfiguration configuration)
        {
            configuration.Register(_serviceRegistration);
        }

        public void Using(IServiceRegistration serviceRegistration)
        {
            if (serviceRegistration.Identity.ServiceType != ServiceType)
                throw new ArgumentException("TODO");

            _serviceRegistration = serviceRegistration;
        }
    }

    public class FluentRegistration<TService> : FluentRegistration, IFluentRegistration<TService>
    {
        public FluentRegistration()
            : base(typeof(TService))
        {
        }
    }
}