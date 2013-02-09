using Domo.DI.Activation;
using Domo.DI.Registration;

namespace Domo.DI.Construction
{
    public class ConstructionFactoryParameter
    {
        public IActivator Activator { get; private set; }

        public ConstructionFactoryParameter(IActivator activator)
        {
            Activator = activator;
        }
    }
}