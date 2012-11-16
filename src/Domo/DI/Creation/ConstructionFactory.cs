using System.Reflection;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Creation
{
    public class ConstructionFactory : IFactory
    {
        private readonly ConstructorInfo _constructor;
        private readonly ConstructionFactoryParameter[] _parameters;

        public ConstructionFactory(ConstructorInfo constructor, ConstructionFactoryParameter[] parameters)
        {
            _constructor = constructor;
            _parameters = parameters;
        }

        public object CreateInstance(ActivationContext activationContext)
        {
            var arguments = _parameters.Convert(parameter => ActivateArgument(activationContext, parameter));

            return _constructor.Invoke(arguments);
        }

        private static object ActivateArgument(ActivationContext activationContext, ConstructionFactoryParameter parameter)
        {
            var activator = parameter.Activator;
            var activationType = parameter.ActivationType;

            return activator.ActivateInstance(activationContext, activationType);
        }
    }
}