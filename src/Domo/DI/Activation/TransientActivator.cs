using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public class TransientActivator : ConstructorActivator
    {
        public TransientActivator(IFactoryContainer factoryContainer, IIdentityManager identityManager)
            : base(factoryContainer, identityManager, null)
        {
        }
    }
}