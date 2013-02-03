using Domo.DI.Caching;
using Domo.DI.Construction;

namespace Domo.DI.Activation
{
    public class SingletonActivator : FactoryActivator
    {
        public SingletonActivator(IFactoryContainer factories, ITypeSubstitution typeSubstitution, IInstanceCache singletonInstanceCache)
            : base(factories, typeSubstitution, singletonInstanceCache)
        {
        }
    }
}