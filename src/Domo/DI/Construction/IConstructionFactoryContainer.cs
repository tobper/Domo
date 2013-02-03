using System;

namespace Domo.DI.Construction
{
    public interface IConstructionFactoryContainer
    {
        IFactory GetFactory(Type serviceType);
    }
}