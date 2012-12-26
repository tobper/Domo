using Domo.DI.Activation;

namespace Domo.DI.Creation
{
    public class ConstructorFactoryParameter
    {
        public IActivator Activator { get; private set; }
        public ServiceIdentity ServiceIdentity { get; private set; }

        public ConstructorFactoryParameter(IActivator activator, ServiceIdentity serviceIdentity)
        {
            Activator = activator;
            ServiceIdentity = serviceIdentity;
        }
    }
}