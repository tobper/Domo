namespace Domo.DI.Registration
{
    public class InstanceServiceConfiguration : ServiceConfiguration
    {
        public InstanceServiceConfiguration(ServiceIdentity identity, object instance) :
            base(identity)
        {
            Instance = instance;
        }

        protected object Instance { get; set; }

        public override IService GetService(IContainer container)
        {
            ProcessCompleteHandlers(container);

            return new InstanceService(Identity, Instance);
        }
    }
}