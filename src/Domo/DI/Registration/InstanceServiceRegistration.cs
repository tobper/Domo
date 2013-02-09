namespace Domo.DI.Registration
{
    public class InstanceServiceRegistration : ServiceRegistration, IInstanceServiceRegistration
    {
        public InstanceServiceRegistration(ServiceIdentity identity, object instance) :
            base(identity)
        {
            Instance = instance;
        }

        public InstanceServiceRegistration(IFluentRegistration fluentRegistration, object instance) :
            base (fluentRegistration)
        {
            Instance = instance;
        }

        public object Instance { get; private set; }

        public override IService GetService(IContainer container)
        {
            ProcessRegisterHandlers(container);

            return new InstanceService(Identity, Instance);
        }
    }
}