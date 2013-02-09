using System;
using Domo.DI.Activation;
using Domo.DI.Construction;

namespace Domo.DI.Registration
{
    public interface IFactoryActivatorRegistration : IActivatorRegistration
    {
        IFactoryActivatorRegistration UsingConcreteType(Type concreteType);
        IFactoryActivatorRegistration UsingConcreteType<TConcrete>();
        IFactoryActivatorRegistration UsingFactory(Func<object> factoryDelegate);
        IFactoryActivatorRegistration UsingFactory(Func<IInjectionContext, object> factoryDelegate);
        IFactoryActivatorRegistration UsingFactory(IFactory factory);
    }

    public interface IFactoryActivatorRegistration<TService> : IActivatorRegistration<TService>
    {
        IFactoryActivatorRegistration<TService> UsingConcreteType(Type concreteType);
        IFactoryActivatorRegistration<TService> UsingConcreteType<TConcrete>() where TConcrete : TService;
        IFactoryActivatorRegistration<TService> UsingFactory(Func<TService> factoryDelegate);
        IFactoryActivatorRegistration<TService> UsingFactory(Func<IInjectionContext, TService> factoryDelegate);
        IFactoryActivatorRegistration<TService> UsingFactory(IFactory factory);
    }
}