using Domo.DI.Activation;

namespace Domo.DI
{
    public class ServiceFamilyMember
    {
        public ServiceIdentity Identity { get; private set; }
        public IActivator Activator { get; private set; }

        public ServiceFamilyMember(ServiceIdentity identity, IActivator activator)
        {
            Identity = identity;
            Activator = activator;
        }
    }
}