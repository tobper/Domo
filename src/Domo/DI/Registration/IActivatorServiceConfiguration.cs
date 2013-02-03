using System;

namespace Domo.DI.Registration
{
    public interface IActivatorServiceConfiguration : IServiceConfiguration
    {
        Type ActivatorType { get; }

        IActivatorServiceConfiguration ActivatedBy(Type activatorType);
    }

    public interface IActivatorServiceConfiguration<TService> : IActivatorServiceConfiguration
    {
    }
}