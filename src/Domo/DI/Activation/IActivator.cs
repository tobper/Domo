namespace Domo.DI.Activation
{
    public interface IActivator
    {
        object ActivateService(IInjectionContext context, ServiceIdentity identity);
    }
}