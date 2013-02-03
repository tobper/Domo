namespace Domo.DI.Registration
{
    public interface IContainerConfiguration
    {
        IContainerConfiguration Register(IServiceConfiguration configuration);

        void CompleteRegistration();
    }
}