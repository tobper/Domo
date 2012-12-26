using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Creation
{
    public class FactoryContainer : IFactoryContainer
    {
        private readonly IContainer _container;
        private readonly IDictionary<Type, IFactory> _factories = new Dictionary<Type, IFactory>();

        public FactoryContainer(IContainer container)
        {
            _container = container;
        }

        public void AddFactory(Type type, IFactory factory)
        {
            _factories.Add(type, factory);
        }

        public IFactory GetFactory(Type type)
        {
            return _factories.TryGetValue(type, CreateConstructorFactory);
        }

        public object CreateInstance(Type type)
        {
            var context = new InjectionContext(_container);
            var instance = CreateInstance(type, context);

            return instance;
        }

        public object CreateInstance(Type type, IInjectionContext context)
        {
            var factory = GetFactory(type);
            var instance = factory.CreateInstance(context);

            return instance;
        }

        private IFactory CreateConstructorFactory(Type type)
        {
            var factories = from constructor in type.GetTypeInfo().DeclaredConstructors
                            let parameters = constructor.GetParameters()
                            orderby parameters.Length descending
                            select CreateConstructorFactory(constructor, parameters);

            var factory = factories.FirstOrDefault();
            if (factory == null)
                throw new NoValidConstructorFoundException(type);

            return factory;
        }

        private IFactory CreateConstructorFactory(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            var factoryParameters = parameters.Convert(parameter =>
            {
                var serviceType = parameter.ParameterType;
                var identity = GetParameterIdentity(serviceType, parameter);
                var activator = _container.GetActivator(identity);

                return new ConstructorFactoryParameter(activator, identity);
            });

            return new ConstructorFactory(constructor, factoryParameters);
        }

        private static ServiceIdentity GetParameterIdentity(Type serviceType, ParameterInfo parameter)
        {
            var identity = serviceType.GetServiceIdentity(parameter.Name);
            if (identity != null)
                return identity;

            // Use default identity if no specific could be created based on parameter name.
            return new ServiceIdentity(serviceType);
        }
    }
}