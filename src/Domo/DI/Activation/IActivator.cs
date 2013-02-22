namespace Domo.DI.Activation
{
    public interface IActivator
    {
        ServiceIdentity Identity { get; }
        object GetService(IInjectionContext context);
    }
}