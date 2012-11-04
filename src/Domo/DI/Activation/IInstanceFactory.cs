namespace Domo.DI.Activation
{
    public interface IInstanceFactory
    {
        object CreateInstance(ActivationContext activationContext);
    }
}