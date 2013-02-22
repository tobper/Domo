using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class TypedServiceExtensions
    {
        public static Func<IInjectionContext, object> GetTypedFuncDelegate(this IActivator activator)
        {
            return CallGenericMethod("GetTypedFuncDelegate", activator.Identity.ServiceType, activator);
        }

        public static Func<IInjectionContext, object> GetTypedLazyDelegate(this IActivator activator)
        {
            return CallGenericMethod("GetTypedLazyDelegate", activator.Identity.ServiceType, activator);
        }

        public static Func<IInjectionContext, object> GetTypedArrayDelegate(this IEnumerable<IActivator> services, Type serviceType)
        {
            return CallGenericMethod("GetTypedArrayDelegate", serviceType, services);
        }

        private static Func<IInjectionContext, object> CallGenericMethod(string methodName, Type genericType, params object[] arguments)
        {
            var method = typeof(TypedServiceExtensions).
                GetTypeInfo().
                DeclaredMethods.Single(m => m.Name == methodName && m.IsGenericMethod).
                MakeGenericMethod(genericType);

            return (Func<IInjectionContext, object>)method.Invoke(null, arguments);
        }

        // ReSharper disable UnusedMember.Local
        private static Func<IInjectionContext, object> GetTypedFuncDelegate<T>(IActivator activator)
        {
            return context => new Func<T>(() => (T)activator.GetService(context));
        }

        private static Func<IInjectionContext, object> GetTypedLazyDelegate<T>(IActivator activator)
        {
            return context => new Lazy<T>(() => (T)activator.GetService((context)));
        }

        private static Func<IInjectionContext, object> GetTypedArrayDelegate<T>(this IEnumerable<IActivator> services)
        {
            return context => services.
                Select(s => s.GetService(context)).
                Cast<T>().
                ToArray();
        }
        // ReSharper restore UnusedMember.Local
    }
}