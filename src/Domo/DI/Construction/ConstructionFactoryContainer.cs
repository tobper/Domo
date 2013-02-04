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
                            let parameters = GetFactoryParameters(constructor)
                            where parameters.All(p => p != null)
                            orderby parameters.Length descending
                            select new ConstructionFactory(constructor, parameters);

            var factory = factories.FirstOrDefault();
            if (factory == null)
                throw new NoValidConstructorFoundException(serviceType);

            return factory;
        }

        private ConstructionFactoryParameter[] GetFactoryParameters(ConstructorInfo constructor)
        {
            var parameters = constructor.GetParameters();

            return parameters.Convert(parameter =>
            {
                var typeInfo = parameter.ParameterType.GetTypeInfo();
                if (typeInfo.IsValueType)
                    return null;

                var serviceIdentity = GetServiceIdentity(parameter.ParameterType, parameter.Name);
                var service = _container.GetService(serviceIdentity);
                if (service == null)
                    return null;

                return new ConstructionFactoryParameter(service);
            });
        }

        private static ServiceIdentity GetServiceIdentity(Type serviceType, string parameterName)
        {
            // First try to get a specific identity based on the parameter name.
            if (false)
            {
                // Todo: Add setting to toggle this functionality
                var identity = serviceType.GetServiceIdentity(parameterName);
                if (identity != null)
                    return identity;
            }

            // Use default identity if no specific could be created based on parameter name.
            return new ServiceIdentity(serviceType);
        }
    }
}