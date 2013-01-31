using Domo.DI.Caching;
using Domo.DI.Creation;

namespace Domo.DI.Activation
{
    public class SingletonActivator : FactoryActivator
    {
        public SingletonActivator(IFactoryContainer factoryContainer, ITypeSubstitution typeSubstitution, IInstanceCache singletonInstanceCache)
            : base(factoryContainer, typeSubstitution, singletonInstanceCache)
        {
        }
    }
}