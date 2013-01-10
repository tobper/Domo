using System;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public interface ITypeRegistration
    {
        ITypeRegistration SetDefaultLifeStyle(LifeStyle lifeStyle);
        ITypeRegistration RegisterDelegate<TService>(Func<IInjectionContext, TService> factoryDelegate, string serviceName = null, LifeStyle lifeStyle = LifeStyle.Default);
        ITypeRegistration RegisterDelegate(Func<IInjectionContext, object> factoryDelegate, ServiceIdentity identity, LifeStyle lifeStyle = LifeStyle.Default);
        ITypeRegistration RegisterSingleton<TService>(TService instance, string serviceName = null) where TService : class;
        ITypeRegistration Register<TService>(string serviceName = null, LifeStyle lifeStyle = LifeStyle.Default);
        ITypeRegistration Register<TService, TInstance>(string serviceName = null, LifeStyle lifeStyle = LifeStyle.Default);
        ITypeRegistration Register(ServiceIdentity identity, LifeStyle lifeStyle = LifeStyle.Default);
        ITypeRegistration Register(ServiceIdentity identity, Type instanceType, LifeStyle lifeStyle = LifeStyle.Default);
        ITypeRegistration RegisterActivator(ServiceIdentity identity, Type activatorType);
    }
}