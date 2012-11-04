namespace Domo.DI.Activation
{
    public interface IServiceActivator
    {
        object ActivateInstance(ActivationContext activationContext);
    }
}