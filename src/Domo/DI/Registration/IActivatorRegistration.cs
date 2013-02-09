namespace Domo.DI.Registration
{
    public interface IActivatorRegistration
    {
        ServiceIdentity Identity { get; }
    }

    public interface IActivatorRegistration<TService>
    {
        ServiceIdentity Identity { get; }
    }
}