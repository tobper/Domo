using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public interface IActivatorConfiguration
    {
        IActivator GetActivator(IContainer container);
    }
}