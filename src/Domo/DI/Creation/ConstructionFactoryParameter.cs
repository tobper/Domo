using System;
using Domo.DI.Activation;

namespace Domo.DI.Creation
{
    public class ConstructionFactoryParameter
    {
        public IActivator Activator { get; private set; }
        public Type ActivationType { get; private set; }

        public ConstructionFactoryParameter(IActivator activator, Type activationType)
        {
            Activator = activator;
            ActivationType = activationType;
        }
    }
}