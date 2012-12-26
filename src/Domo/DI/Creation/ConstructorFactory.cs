using System.Reflection;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Creation
{
    public class ConstructorFactory : IFactory
    {
        private readonly ConstructorInfo _constructor;
        private readonly ConstructorFactoryParameter[] _parameters;

        public ConstructorFactory(ConstructorInfo constructor, ConstructorFactoryParameter[] parameters)
        {
            _constructor = constructor;
            _parameters = parameters;
        }

        public object CreateInstance(IInjectionContext context)
        {
            var arguments = _parameters.Convert(parameter => ActivateArgument(context, parameter));

            return _constructor.Invoke(arguments);
        }

        private static object ActivateArgument(IInjectionContext context, ConstructorFactoryParameter parameter)
        {
            var activator = parameter.Activator;
            var identity = parameter.ServiceIdentity;

            return activator.ActivateService(context, identity);
        }
    }
}