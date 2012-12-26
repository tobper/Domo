using Domo.DI.Caching;
using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public class SingletonActivator : ConstructorActivator
    {
        public SingletonActivator(IFactoryContainer factoryContainer, IIdentityManager identityManager, IInstanceCache singletonInstanceCache)
            : base(factoryContainer, identityManager, singletonInstanceCache)
        {
        }
    }
}