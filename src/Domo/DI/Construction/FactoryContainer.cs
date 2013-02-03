using System.Collections.Generic;
using Domo.DI.Activation;
using Domo.Extensions;

namespace Domo.DI.Construction
{
    public class FactoryContainer : IFactoryContainer
    {
        private readonly IContainer _container;
        private readonly IConstructionFactoryContainer _constructionFactories;
        private readonly IDictionary<ServiceIdentity, IFactory> _factories = new Dictionary<ServiceIdentity, IFactory>();

        public FactoryContainer(IContainer container, IConstructionFactoryContainer constructionFactories)
        {
            _container = container;
            _constructionFactories = constructionFactories;
        }

        public void AddFactory(ServiceIdentity identity, IFactory factory)
        {
            _factories.Add(identity, factory);
        }

        public IFactory GetFactory(ServiceIdentity identity)
        {
            return _factories.TryGetValue(identity, GetConstructionFactory);
        }

        private IFactory GetConstructionFactory(ServiceIdentity identity)
        {
            return _constructionFactories.GetFactory(identity.ServiceType);
        }

        public object CreateInstance(ServiceIdentity identity)
        {
            var context = new InjectionContext(_container);
            var instance = CreateInstance(identity, context);

            return instance;
        }

        public object CreateInstance(ServiceIdentity identity, IInjectionContext context)
        {
            var factory = GetFactory(identity);
            var instance = factory.CreateInstance(context);

            return instance;
        }
    }
}