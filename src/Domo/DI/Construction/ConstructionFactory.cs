using System.Reflection;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Construction
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

        public object CreateInstance(IInjectionContext context)
        {
            var arguments = _parameters.Convert(parameter => ActivateArgument(context, parameter));

            return _constructor.Invoke(arguments);
        }

        private static object ActivateArgument(IInjectionContext context, ConstructionFactoryParameter parameter)
        {
            return parameter.ActivationDelegate(context);
        }
    }
}