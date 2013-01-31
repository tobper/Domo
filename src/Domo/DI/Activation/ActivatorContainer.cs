using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Creation;
using Domo.Extensions;

namespace Domo.DI.Activation
{
    public class ActivatorContainer : IActivatorContainer
    {
        private readonly IDictionary<Type, IActivator> _activators;
        private readonly IFactoryContainer _factoryContainer;

        public ActivatorContainer(IFactoryContainer factoryContainer, params IActivator[] defaultActivators)
        {
            _factoryContainer = factoryContainer;
            _activators = defaultActivators.ToDictionary(
                a => a.GetType(),
                a => a);
        }

        public IActivator GetActivator(Type activatorType)
        {
            return _activators.TryGetValue(activatorType, CreateActivator);
        }

        private IActivator CreateActivator(Type activatorType)
        {
            return (IActivator)_factoryContainer.CreateInstance(activatorType);
        }
    }
}