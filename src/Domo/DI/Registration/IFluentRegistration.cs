namespace Domo.DI.Registration
{
    public interface IFluentRegistration
    {
        ServiceIdentity Identity { get; }

        void Using(IActivatorConfiguration activatorConfiguration);
    }

    public interface IFluentRegistration<TService>
    {
        ServiceIdentity Identity { get; }

        void Using(IActivatorConfiguration activatorConfiguration);
    }
}