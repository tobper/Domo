namespace Domo.DI.Activation
{
    public class InstanceActivator : IActivator
    {
        public InstanceActivator(ServiceIdentity identity, object instance)
        {
            Identity = identity;
            Instance = instance;
        }

        public ServiceIdentity Identity { get; private set; }
        public object Instance { get; private set; }

        public object GetService(IInjectionContext context)
        {
            return Instance;
        }
    }
}