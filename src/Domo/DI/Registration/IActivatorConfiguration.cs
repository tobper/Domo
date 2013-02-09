using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public interface IActivatorConfiguration
    {
        IActivator GetService(IContainer container);
    }
}