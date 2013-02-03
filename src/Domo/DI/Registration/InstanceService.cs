using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public class InstanceService : IService
    {
        public InstanceService(ServiceIdentity identity, object instance)
        {
            Identity = identity;
            Instance = instance;
        }

        public ServiceIdentity Identity { get; private set; }
        public object Instance { get; private set; }

        public object GetInstance(IInjectionContext context)
        {
            return Instance;
        }
    }
}