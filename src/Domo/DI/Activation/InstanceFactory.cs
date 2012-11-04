using System.Reflection;
using Domo.Extensions;

namespace Domo.DI.Activation
{
    public class InstanceFactory : IInstanceFactory
    {
        private readonly ConstructorInfo _constructor;
        private readonly IServiceActivator[] _activators;

        public InstanceFactory(ConstructorInfo constructor, IServiceActivator[] activators)
        {
            _constructor = constructor;
            _activators = activators;
        }

        public object CreateInstance(ActivationContext activationContext)
        {
            var arguments = _activators.Convert(a => a.ActivateInstance(activationContext));
            return _constructor.Invoke(arguments);
        }
    }
}