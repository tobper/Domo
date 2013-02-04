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
            var arguments = _parameters.Convert(parameter => GetInstance(context, parameter));
            var instance = _constructor.Invoke(arguments);

            return instance;
        }

        private static object GetInstance(IInjectionContext context, ConstructionFactoryParameter parameter)
        {
            return parameter.Service.GetInstance(context);
        }
    }
}