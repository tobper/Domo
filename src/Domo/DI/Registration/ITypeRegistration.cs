using System;

namespace Domo.DI.Registration
{
    public interface ITypeRegistration
    {
        ITypeRegistration SetDefaultLifeStyle(LifeStyle lifeStyle);
        ITypeRegistration RegisterSingleton<TService>(TService instance, string serviceName = null) where TService : class;
        ITypeRegistration Register<TService>(LifeStyle lifeStyle = LifeStyle.Default, string serviceName = null);
        ITypeRegistration Register<TService, TInstance>(LifeStyle lifeStyle = LifeStyle.Default, string serviceName = null);
        ITypeRegistration Register(Type serviceType, Type instanceType = null, LifeStyle lifeStyle = LifeStyle.Default, string serviceName = null);
        ITypeRegistration RegisterActivator(Type serviceType, Type activatorType, string serviceName = null);
    }
}