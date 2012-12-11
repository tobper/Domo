using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using Domo.Extensions;

namespace Domo.DI.Creation
{
    public class FactoryManager : IFactoryManager
    {
        private readonly IContainer _container;
        private readonly IDictionary<Type, IFactory> _factories = new Dictionary<Type, IFactory>();

        public FactoryManager(IContainer container)
        {
            _container = container;
        }

        public void AddFactory(Type type, IFactory factory)
        {
            _factories.Add(type, factory);
        }

        public IFactory GetFactory(Type type)
        {
            return _factories.TryGetValue(type, CreateFactory);
        }

        private IFactory CreateFactory(Type type)
        {
            var factories = from constructor in type.GetTypeInfo().DeclaredConstructors
                            let parameters = constructor.GetParameters()
                            orderby parameters.Length descending
                            select CreateFactory(constructor, parameters);

            var factory = factories.FirstOrDefault();
            if (factory == null)
                throw new NoValidConstructorFoundException(type);

            return factory;
        }

        private IFactory CreateFactory(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            var factoryParameters = parameters.Convert(parameter =>
            {
                var serviceType = parameter.ParameterType;
                var serviceName = GetServiceName(serviceType, parameter.Name);
                var activator = _container.GetActivator(serviceType, serviceName);

                return new ConstructionFactoryParameter(activator, serviceType, serviceName);
            });

            return new ConstructionFactory(constructor, factoryParameters);
        }

        private string GetServiceName(Type serviceType, string name)
        {
            var serviceTypeName = Regex.Replace(serviceType.Name, "^I", string.Empty);
            var match = Regex.Match(name, "i?(.*?)" + serviceTypeName);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return null;
        }
    }
}