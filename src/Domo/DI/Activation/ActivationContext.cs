namespace Domo.DI.Activation
{
    // Todo: Rename to avoid mismatch with System.ActivationContext
    public class ActivationContext
    {
        public IContainer Container { get; set; }

        public ActivationContext(IContainer container)
        {
            Container = container;
        }
    }
}