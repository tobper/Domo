using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domo.Extensions;

namespace Domo.DI.Construction
{
    public class ConstructionFactoryContainer : IConstructionFactoryContainer
    {
        private readonly IContainer _container;
        private readonly IDictionary<Type, IFactory> _factories;

        public ConstructionFactoryContainer(IContainer container)
        {
            _container = container;
            _factories = new Dictionary<Type, IFactory>();
        }

        public IFactory GetFactory(Type serviceType)
        {
            return _factories.TryGetValue(serviceType, CreateFactory);
        }

        private IFactory CreateFactory(Type serviceType)
        {
            var factories = from constructor in serviceType.GetTypeInfo().DeclaredConstructors
                            let parameters = constructor.GetParameters()
                            where !parameters.Any(p => p.ParameterType.GetTypeInfo().IsValueType)
                            orderby parameters.Length descending
                            select CreateConstructionFactory(constructor, parameters);

            var factory = factories.FirstOrDefault();
            if (factory == null)
                throw new NoValidConstructorFoundException(serviceType);

            return factory;
        }

        private IFactory CreateConstructionFactory(ConstructorInfo constructor, ParameterInfo[] parameters)
        {
            var constructorParameters = parameters.Convert(parameter =>
            {
                var serviceIdentity = GetParameterServiceIdentity(parameter.ParameterType, parameter.Name);
                var service = _container.GetService(serviceIdentity);
                if (service == null)
                    throw new ServiceNotRegisteredException(serviceIdentity);

                return new ConstructionFactoryParameter(service);
            });

            return new ConstructionFactory(constructor, constructorParameters);
        }

        private static ServiceIdentity GetParameterServiceIdentity(Type serviceType, string parameterName)
        {
            // First try to get a specific identity based on the parameter name.
            var identity = serviceType.GetServiceIdentity(parameterName);
            if (identity != null)
                return identity;

            // Use default identity if no specific could be created based on parameter name.
            return new ServiceIdentity(serviceType);
        }
    }
}