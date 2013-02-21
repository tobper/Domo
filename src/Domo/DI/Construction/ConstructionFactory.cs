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
        public ConstructionFactoryParameter[] Parameters { get; private set; }
        public ConstructionFactoryProperty[] Properties { get; private set; }

        public ConstructionFactory(Type constructedType)
        {
            ConstructedType = constructedType;
        }

        private ConstructionFactory(ConstructorInfo constructor, ConstructionFactoryParameter[] parameters, ConstructionFactoryProperty[] properties)
        {
            ConstructedType = constructor.DeclaringType;
            Constructor = constructor;
            Parameters = parameters;
            Properties = properties;
        }

        public object CreateInstance(IInjectionContext context)
        {
            EnsureInitialized(context);

            var arguments = Parameters.Convert(p => p.Activator.GetInstance(context));
            var instance = Constructor.Invoke(arguments);

            foreach (var property in Properties)
            {
                property.Set(instance, context);
            }

            return instance;
        }

        public static ConstructionFactory Create(Type type, IContainer container)
        {
            var factory = TryCreate(type, container);
            if (factory == null)
                throw new NoValidConstructorFoundException(type);

            return factory;
        }

        public static ConstructionFactory TryCreate(Type type, IContainer container)
        {
            var settings =
                container.ServiceLocator.TryResolve<ConstructionFactorySettings>() ??
                new ConstructionFactorySettings();

            var typeInfo = type.GetTypeInfo();
            var candidates =
                from constructor in typeInfo.DeclaredConstructors
                let parameters = constructor.GetParameters()
                orderby parameters.Length descending
                select new
                {
                    Constructor = constructor,
                    Parameters = parameters
                };

            foreach (var candidate in candidates)
            {
                var parameters = GetConstructorParameters(candidate.Parameters, container, settings);
                if (parameters.Any(p => p == null))
                    continue;

                var properties = GetProperties(typeInfo, container, settings);
                var factory = new ConstructionFactory(candidate.Constructor, parameters, properties);

                return factory;
            }

            return null;
        }

        private static ConstructionFactoryParameter[] GetConstructorParameters(ParameterInfo[] parameters, IContainer container, ConstructionFactorySettings settings)
        {
            return parameters.Convert(parameter =>
            {
                var activator = TryGetActivator(parameter.ParameterType, parameter.Name, container, settings);
                if (activator == null)
                    return null;

                return new ConstructionFactoryParameter(activator);
            });
        }

        private static ConstructionFactoryProperty[] GetProperties(TypeInfo typeInfo, IContainer container, ConstructionFactorySettings settings)
        {
            if (!settings.UsePropertyInjection)
                return new ConstructionFactoryProperty[0];

            var properties = typeInfo.DeclaredProperties.
                Select(property =>
                {
                    if (settings.PropertyInjectionAttribute != null)
                    {
                        if (property.IsDefined(settings.PropertyInjectionAttribute) == false)
                            return null;
                    }
                    else
                    {
                        if (!property.GetMethod.IsPublic ||
                            !property.SetMethod.IsPublic)
                            return null;
                    }

                    var activator = TryGetActivator(property.PropertyType, property.Name, container, settings);
                    if (activator == null)
                        return null;

                    return new ConstructionFactoryProperty(activator, property);
                }).
                Where(p => p != null).
                ToArray();

            return properties;
        }

        private static ServiceIdentity GetServiceIdentity(Type serviceType, string referenceName, ConstructionFactorySettings settings)
        {
            // First try to get a specific identity based on the parameter name.
            if (settings.UsePrefixResolution)
            {
                var identity = serviceType.TryGetServiceIdentity(referenceName);
                if (identity != null)
                    return identity;
            }

            // Use default identity if no specific could be created based on parameter name.
            return new ServiceIdentity(serviceType);
        }

        private static IActivator TryGetActivator(Type serviceType, string referenceName, IContainer container, ConstructionFactorySettings settings)
        {
            var typeInfo = serviceType.GetTypeInfo();
            if (typeInfo.IsValueType)
                return null;

            var serviceIdentity = GetServiceIdentity(serviceType, referenceName, settings);
            var activator = container.GetActivator(serviceIdentity);
            if (activator == null)
                return null;

            return activator;
        }

        private void EnsureInitialized(IInjectionContext context)
        {
            if (Constructor == null)
            {
                lock (_initializationLock)
                {
                    if (Constructor == null)
                    {
                        var factory = Create(ConstructedType, context.Container);

                        Constructor = factory.Constructor;
                        Parameters = factory.Parameters;
                        Properties = factory.Properties;
                    }
                }
            }
        }
    }
}