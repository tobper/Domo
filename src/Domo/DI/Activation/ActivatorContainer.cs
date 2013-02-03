using System;
using System.Collections.Generic;
using System.Linq;
using Domo.DI.Construction;
using Domo.Extensions;

namespace Domo.DI.Activation
{
    public class ActivatorContainer : IActivatorContainer
    {
        private readonly IDictionary<Type, IActivator> _activators;
        private readonly IFactoryContainer _factories;

        public ActivatorContainer(IFactoryContainer factories, params IActivator[] defaultActivators)
        {
            _factories = factories;
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
            var identity = new ServiceIdentity(activatorType);

            return (IActivator)_factories.CreateInstance(identity);
        }
    }
}