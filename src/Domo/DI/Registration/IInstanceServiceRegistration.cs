namespace Domo.DI.Registration
{
    public interface IInstanceServiceRegistration : IServiceRegistration
    {
        object Instance { get; }
    }
}