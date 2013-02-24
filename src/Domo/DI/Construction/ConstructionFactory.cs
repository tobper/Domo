using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Construction
{
    public class ConstructionFactory : IFactory
    {
        private readonly static MethodInfo ActivationMethod;
        private readonly object _initializationLock = new object();

        private Func<IInjectionContext, object> _factoryDelegate;

        public Type ConstructionType { get; private set; }

        static ConstructionFactory()
        {
            ActivationMethod = typeof(IActivator).
                GetTypeInfo().
                DeclaredMethods.Single(m => m.Name == "ActivateService");
        }

        public ConstructionFactory(Type constructionType)
        {
            ConstructionType = constructionType;
        }

        private ConstructionFactory(Type constructionType, Func<IInjectionContext, object> factoryDelegate)
        {
            ConstructionType = constructionType;
            _factoryDelegate = factoryDelegate;
        }

        public object CreateService(IInjectionContext context)
        {
            EnsureInitialized(context);

            return _factoryDelegate(context);
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
            var factoryDelegate = TryCreateFactoryDelegate(type, container);
            if (factoryDelegate == null)
                return null;

            return new ConstructionFactory(type, factoryDelegate);
        }

        private void EnsureInitialized(IInjectionContext context)
        {
            if (_factoryDelegate != null)
                return;

            lock (_initializationLock)
            {
                if (_factoryDelegate != null)
                    return;

                _factoryDelegate = TryCreateFactoryDelegate(ConstructionType, context.Container);

                if (_factoryDelegate == null)
                    throw new NoValidConstructorFoundException(ConstructionType);
            }
        }

        private static Func<IInjectionContext, object> TryCreateFactoryDelegate(Type type, IContainer container)
        {
            var settings =
                container.ServiceLocator.TryResolve<ConstructionFactorySettings>() ??
                new ConstructionFactorySettings();

            var typeInfo = type.GetTypeInfo();
            var constructor = TryGetConstructorDetails(typeInfo, container, settings);
            if (constructor == null)
                return null;

            var contextExpression = Expression.Parameter(typeof(IInjectionContext));
            var constructionExpression = CreateConstructionExpression(constructor.ConstructorInfo, constructor.Activators, contextExpression);

            if (settings.UsePropertyInjection)
            {
                var serviceExpression = Expression.Variable(typeInfo.AsType());
                var expressions = new List<Expression>
                {
                    Expression.Assign(serviceExpression, constructionExpression)
                };

                foreach (var property in typeInfo.DeclaredProperties)
                {
                    var activator = TryGetPropertyActivator(property, container, settings);
                    if (activator == null)
                        continue;

                    var propertyValueExpression = CreateActivationExpression(activator, contextExpression);
                    var setterExpression = Expression.Call(serviceExpression, property.SetMethod, new[] { propertyValueExpression });

                    if (settings.PropertyInjectionAttribute != null)
                    {
                        // Always set property when attribute is used
                        expressions.Add(setterExpression);
                    }
                    else
                    {
                        // Only set property if it is null when attribute isn't used
                        expressions.Add(
                            Expression.Condition(
                                Expression.Equal(
                                    Expression.Constant(null),
                                    Expression.Call(serviceExpression, property.GetMethod)),
                                setterExpression,
                                Expression.Default(property.SetMethod.ReturnType)));
                    }
                }

                expressions.Add(serviceExpression);
                constructionExpression = Expression.Block(new[] { serviceExpression }, expressions);
            }

            return Expression.
                Lambda<Func<IInjectionContext, object>>(constructionExpression, contextExpression).
                Compile();
        }

        private static ConstructorDetails TryGetConstructorDetails(TypeInfo typeInfo, IContainer container, ConstructionFactorySettings settings)
        {
            var candidates =
                from constructor in typeInfo.DeclaredConstructors
                let parameters = constructor.GetParameters()
                orderby parameters.Length descending
                let activators = TryGetParameterActivators(parameters, container, settings)
                where activators != null
                select new ConstructorDetails
                {
                    ConstructorInfo = constructor,
                    Parameters = parameters,
                    Activators = activators
                };

            return candidates.FirstOrDefault();
        }

        private static Expression CreateConstructionExpression(ConstructorInfo constructor, IActivator[] activators, ParameterExpression contextExpression)
        {
            var arguments = activators.Convert(
                activator =>
                CreateActivationExpression(activator, contextExpression));

            return Expression.New(constructor, arguments);
        }

        private static Expression CreateActivationExpression(IActivator activator, ParameterExpression contextExpression)
        {
            var methodCall = Expression.Call(
                Expression.Constant(activator),
                ActivationMethod,
                new Expression[] { contextExpression });

            return Expression.Convert(methodCall, activator.Identity.ServiceType);
        }

        private static IActivator[] TryGetParameterActivators(ParameterInfo[] parameters, IContainer container, ConstructionFactorySettings settings)
        {
            var activators = new IActivator[parameters.Length];

            for (var i = 0; i < parameters.Length; i++)
            {
                var parameter = parameters[i];
                var activator = TryGetActivator(parameter.ParameterType, parameter.Name, container, settings);
                if (activator == null)
                    return null;

                activators[i] = activator;
            }

            return activators;
        }

        private static IActivator TryGetPropertyActivator(PropertyInfo property, IContainer container, ConstructionFactorySettings settings)
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

            return TryGetActivator(property.PropertyType, property.Name, container, settings);
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

        class ConstructorDetails
        {
            public ConstructorInfo ConstructorInfo { get; set; }
            public ParameterInfo[] Parameters { get; set; }
            public IActivator[] Activators { get; set; }
        }
    }
}