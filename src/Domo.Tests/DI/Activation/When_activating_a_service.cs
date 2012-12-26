using NUnit.Framework;

namespace Domo.Tests.DI.Activation
{
    [TestFixture]
    [Ignore]
    public class When_activating_a_new_service : SingletonActivatorTests
    {
        [Test]
        public void The_singleton_cache_should_be_asked_for_an_instance()
        {
        }

        [Test]
        public void The_factory_delegate_should_be_asked_to_create_an_instance()
        {
            
        }

        protected override void SetupPrerequisites()
        {
            GivenAValidActivationContext();
            GivenTheSingletonCacheAsksTheFactoryDelegateForAnInstance();
        }
    }
}
