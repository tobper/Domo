using Domo.DI.Caching;
using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public class SingletonActivator : ConstructorActivator
    {
        public SingletonActivator(IFactoryContainer factoryContainer, ITypeSubstitution typeSubstitution, IInstanceCache singletonInstanceCache)
            : base(factoryContainer, typeSubstitution, singletonInstanceCache)
        {
        }
    }
}