using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public interface IService
    {
        ServiceIdentity Identity { get; }
        ActivationDelegate GetActivationDelegate();
    }
}