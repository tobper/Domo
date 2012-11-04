namespace Domo.DI.Activation
{
    public class TransientServiceActivator : IServiceActivator
    {
        private readonly IInstanceFactory _instanceFactory;

        public TransientServiceActivator(IInstanceFactory instanceFactory)
        {
            _instanceFactory = instanceFactory;
        }

        public object ActivateInstance(ActivationContext activationContext)
        {
            return _instanceFactory.CreateInstance(activationContext);
        }
    }
}