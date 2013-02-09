namespace Domo.DI.Activation
{
    public interface IActivator
    {
        ServiceIdentity Identity { get; }
        object GetInstance(IInjectionContext context);
    }
}