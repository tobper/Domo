using Domo.DI.Activation;

namespace Domo.DI.Construction
{
    public class ConstructionFactoryParameter
    {
        public ActivationDelegate ActivationDelegate { get; private set; }

        public ConstructionFactoryParameter(ActivationDelegate activationDelegate)
        {
            ActivationDelegate = activationDelegate;
        }
    }
}