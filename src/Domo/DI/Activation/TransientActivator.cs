using Domo.DI.Construction;

namespace Domo.DI.Activation
{
    public class TransientActivator : FactoryActivator
    {
        public TransientActivator(IFactoryContainer factories, ITypeSubstitution typeSubstitution)
            : base(factories, typeSubstitution, null)
        {
        }
    }
}