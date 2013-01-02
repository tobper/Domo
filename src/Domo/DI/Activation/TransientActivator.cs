using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public class TransientActivator : ConstructorActivator
    {
        public TransientActivator(IFactoryContainer factoryContainer, ITypeSubstitution typeSubstitution)
            : base(factoryContainer, typeSubstitution, null)
        {
        }
    }
}