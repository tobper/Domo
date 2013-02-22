using System;

namespace Domo.DI.Construction
{
    public static class TypeExtensions
    {
        public static object ConstructInstance(this Type type, IContainer container)
        {
            var factory = ConstructionFactory.Create(type, container);
            var context = new InjectionContext(container);
            var instance = factory.CreateService(context);

            return instance;
        }
    }
}