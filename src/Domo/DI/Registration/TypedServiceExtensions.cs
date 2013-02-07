using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class TypedServiceExtensions
    {
        public static Func<IInjectionContext, object> GetTypedFuncDelegate(this IService service)
        {
            return CallGenericMethod("GetTypedFuncDelegate", service.Identity.ServiceType, service);
        }

        public static Func<IInjectionContext, object> GetTypedLazyDelegate(this IService service)
        {
            return CallGenericMethod("GetTypedLazyDelegate", service.Identity.ServiceType, service);
        }

        public static Func<IInjectionContext, object> GetTypedArrayDelegate(this IEnumerable<IService> services, Type serviceType)
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
        private static Func<IInjectionContext, object> GetTypedFuncDelegate<T>(IService service)
        {
            return context => new Func<T>(() => (T)service.GetInstance(context));
        }

        private static Func<IInjectionContext, object> GetTypedLazyDelegate<T>(IService service)
        {
            return context => new Lazy<T>(() => (T)service.GetInstance((context)));
        }

        private static Func<IInjectionContext, object> GetTypedArrayDelegate<T>(this IEnumerable<IService> services)
        {
            return context => services.
                Select(s => s.GetInstance(context)).
                Cast<T>().
                ToArray();
        }
        // ReSharper restore UnusedMember.Local
    }
}