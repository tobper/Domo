namespace Domo.DI.Registration
{
    public interface IInstanceActivatorRegistration : IActivatorRegistration
    {
        object Instance { get; }
    }

    public interface IInstanceActivatorRegistration<TService> : IActivatorRegistration<TService>
    {
        TService Instance { get; }
    }
}