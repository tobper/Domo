using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public class TransientActivator : FactoryActivator
    {
        public TransientActivator(IFactoryContainer factoryContainer, ITypeSubstitution typeSubstitution)
            : base(factoryContainer, typeSubstitution, null)
        {
        }
    }
}