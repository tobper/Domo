using NUnit.Framework;

namespace Domo.Tests.DI.Activation.SingletonFactoryActivatorTests
{
    [TestFixture]
    public class When_activating_a_service : SingletonFactoryActivatorTests
    {
        [Test]
        public void The_singleton_cache_should_be_asked_for_an_instance()
        {
            SingletonCache.VerifyAll();
        }

        protected override void SetupPrerequisites()
        {
            GivenTheCacheReturnsAnInstance();
            GivenAValidActivationContext();
        }
    }
}
