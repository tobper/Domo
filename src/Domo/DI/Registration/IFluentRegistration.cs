namespace Domo.DI.Registration
{
    public interface IFluentRegistration
    {
        ServiceIdentity Identity { get; }
        IActivatorConfiguration Configuration { get; set; }
    }

    public interface IFluentRegistration<TService>
    {
        ServiceIdentity Identity { get; }
        IActivatorConfiguration Configuration { get; set; }
    }
}