using System;
using System.Reflection;
using System.Web.Http;
using Domo.DI;
using Domo.DI.Registration;

namespace Domo.WebApi
{
    public class DomoApiControllerScanProcessor : IScanProcessor
    {
        private readonly Type _controllerType;

        public DomoApiControllerScanProcessor()
        {
            _controllerType = typeof(ApiController);
        }

        public void ProcessType(ITypeRegistration typeRegistration, TypeInfo typeInfo)
        {
            if (typeInfo.IsAbstract)
                return;

            if (_controllerType.IsAssignableFrom(typeInfo))
            {
                var type = typeInfo.AsType();
                var identity = new ServiceIdentity(type);

                typeRegistration.Register(identity, LifeStyle.Transient);
            }
        }
    }
}
