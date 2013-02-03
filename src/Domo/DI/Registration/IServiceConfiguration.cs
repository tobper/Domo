using System;

namespace Domo.DI.Registration
{
    public interface IServiceConfiguration
    {
        ServiceIdentity Identity { get; }

        IService GetService(IContainer container);
        IServiceConfiguration OnComplete(Action<IContainer> onComplete);
        IServiceConfiguration WithName(string name);
    }
}