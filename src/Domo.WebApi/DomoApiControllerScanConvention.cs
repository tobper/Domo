using System;
using System.Reflection;
using System.Web.Http;
using Domo.DI.Registration;

namespace Domo.WebApi
{
    public class DomoApiControllerScanConvention : IScanConvention
    {
        private readonly Type _controllerType;

        public DomoApiControllerScanConvention()
        {
            _controllerType = typeof(ApiController);
        }

        public void ProcessType(IContainerConfiguration container, TypeInfo type)
        {
            if (type.IsAbstract || type.IsInterface || type.IsGenericTypeDefinition || type.IsNested)
                return;

            ProcessConcreteType(container, type);
        }

        private void ProcessConcreteType(IContainerConfiguration container, TypeInfo concreteType)
        {
            if (_controllerType.IsAssignableFrom(concreteType))
            {
                var serviceType = concreteType.AsType();

                container.
                    Register(serviceType).
                    AsTransient().
                    UsingConcreteType(concreteType);
            }
        }
    }
}
