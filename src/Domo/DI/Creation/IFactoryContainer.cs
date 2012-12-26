using System;
using Domo.DI.Activation;

namespace Domo.DI.Creation
{
    public interface IFactoryContainer
    {
        void AddFactory(Type type, IFactory factory);
        object CreateInstance(Type type);
        object CreateInstance(Type type, IInjectionContext context);
    }
}