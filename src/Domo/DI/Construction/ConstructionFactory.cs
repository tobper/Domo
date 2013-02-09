using System;
using System.Linq;
using System.Reflection;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Construction
{
    public class ConstructionFactory : IFactory
    {
        private readonly object _initializationLock = new object();

        public Type ConstructedType { get; private set; }
        public ConstructorInfo Constructor { get; private set; }
        public ConstructionFactoryParameter[] ConstructorParameters { get; private set; }

        public ConstructionFactory(Type constructedType)
        {
            ConstructedType = constructedType;
        }

        private ConstructionFactory(ConstructorInfo constructor, ConstructionFactoryParameter[] parameters)
        {
            ConstructedType = constructor.DeclaringType;
            Constructor = constructor;
            ConstructorParameters = parameters;
        }

        public object CreateInstance(IInjectionContext context)
        {
            if (Constructor == null)
            {
                lock (_initializationLock)
                {
                    if (Constructor == null)
                    {
                        var factory = Create(ConstructedType, context.Container);

                        Constructor = factory.Constructor;
                        ConstructorParameters = factory.ConstructorParameters;
                    }
                }
            }

            var arguments = ConstructorParameters.Convert(p => p.Activator.GetInstance(context));
            var instance = Constructor.Invoke(arguments);

            return instance;
        }

        public static ConstructionFactory Create(Type type, IContainer container)
        {
            var factory = TryCreate(type, container);
            if (factory == null)
                throw new NoValidConstructorFoundException(type);

            return factory;
        }

        public static ConstructionFactory CreateDelayed(Type type)
        {
            return new ConstructionFactory(type);
        }

        public static ConstructionFactory TryCreate(Type type, IContainer container)
        {
            var factories =
                from constructor in type.GetTypeInfo().DeclaredConstructors
                let constructorParameters = constructor.GetParameters()
                orderby constructorParameters.Length descending
                let factoryParameters = GetConstructorParameters(constructorParameters, container)
                where factoryParameters.All(p => p != null)
                select new ConstructionFactory(constructor, factoryParameters);

            return factories.FirstOrDefault();
        }

        private static ConstructionFactoryParameter[] GetConstructorParameters(ParameterInfo[] parameters, IContainer container)
        {
            return parameters.Convert(parameter =>
            {
                var typeInfo = parameter.ParameterType.GetTypeInfo();
                if (typeInfo.IsValueType)
                    return null;

                var serviceIdentity = GetServiceIdentity(parameter.ParameterType, parameter.Name);
                var service = container.GetService(serviceIdentity);
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
                var identity = serviceType.TryGetServiceIdentity(parameterName);
                if (identity != null)
                    return identity;
            }

            // Use default identity if no specific could be created based on parameter name.
            return new ServiceIdentity(serviceType);
        }
    }
}