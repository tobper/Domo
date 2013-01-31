using Domo.DI.Activation;

namespace Domo.DI.Creation
{
    public class ConstructorFactoryParameter
    {
        public ActivationDelegate ActivationDelegate { get; private set; }

        public ConstructorFactoryParameter(ActivationDelegate activationDelegate)
        {
            ActivationDelegate = activationDelegate;
        }
    }
}