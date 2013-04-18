using System;
using Domo.DI;
using Domo.DI.Activation;
using Domo.DI.Construction;
using Domo.DI.Registration;
using Domo.Data.DI.Activation;

namespace Domo.Data.DI.Registration
{
    public class ServiceLoaderActivatorConfiguration : IActivatorConfiguration
    {
        private readonly Type _dataType;

        public ServiceLoaderActivatorConfiguration(Type dataType)
        {
            _dataType = dataType;
        }

        public IActivator GetActivator(IContainer container)
        {
            var activatorType = typeof(ServiceLoaderActivator<>).MakeGenericType(_dataType);
            var activator = (IActivator)activatorType.ConstructInstance(container);

            return activator;
        }
    }
}