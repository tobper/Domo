using NUnit.Framework;

namespace Domo.Tests.DI.Activation.SingletonActivatorTests
{
    [TestFixture]
    public class When_activating_a_service : SingletonActivatorTests
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
