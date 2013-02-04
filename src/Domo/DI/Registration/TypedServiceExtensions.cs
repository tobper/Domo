using System;
using System.Linq;
using System.Reflection;
using Domo.DI.Activation;

namespace Domo.DI.Registration
{
    public static class TypedServiceExtensions
    {
        public static Func<IInjectionContext, object> GetTypedFuncDelegate(this IService service)
        {
            return CallGenericMethod("GetTypedFuncDelegate", service);
        }

        public static Func<IInjectionContext, object> GetTypedLazyDelegate(this IService service)
        {
            return CallGenericMethod("GetTypedLazyDelegate", service);
        }

        private static Func<IInjectionContext, object> CallGenericMethod(string methodName, IService service)
        {
            var parameters = new object[] { service };
            var method = typeof(TypedServiceExtensions).
                GetTypeInfo().
                DeclaredMethods.Single(m => m.Name == methodName && m.IsGenericMethod).
                MakeGenericMethod(service.Identity.ServiceType);

            return (Func<IInjectionContext, object>)method.Invoke(null, parameters);
        }

        private static Func<IInjectionContext, object> GetTypedFuncDelegate<T>(IService service)
        {
            return context => new Func<T>(() => (T)service.GetInstance(context));
        }

        private static Func<IInjectionContext, object> GetTypedLazyDelegate<T>(IService service)
        {
            return context => new Lazy<T>(() => (T)service.GetInstance((context)));
        }
    }
}