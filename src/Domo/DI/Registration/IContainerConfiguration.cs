namespace Domo.DI.Registration
{
    public interface IContainerConfiguration
    {
        IContainerConfiguration Register(IServiceConfiguration serviceConfiguration);

        void CompleteRegistration();
    }
}