using System;

namespace Domo.DI.Creation
{
    public interface IFactoryManager
    {
        void AddFactory(Type type, IFactory factory);
        IFactory GetFactory(Type type);
    }
}