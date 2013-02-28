using System.Reflection;
using Domo.DI.Caching;

namespace Domo.DI.Registration
{
    public static class TypeExtensions
    {
        public static IServiceScope GetServiceScope(this TypeInfo type)
        {
            var serviceTypeAttribute = type.GetCustomAttribute<ServiceScopeAttribute>();
            if (serviceTypeAttribute != null)
                return serviceTypeAttribute.Scope;

            return null;
        }
    }
}