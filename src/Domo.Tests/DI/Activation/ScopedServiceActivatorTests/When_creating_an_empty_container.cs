using NUnit.Framework;

namespace Domo.Tests.DI.Activation.ScopedServiceActivatorTests
{
    [TestFixture]
    public class When_activating_a_service : ScopedServiceActivatorTests
    {
        [Test]
        public void The_scoped_cache_should_be_asked_for_an_instance()
        {
            ScopedCache.VerifyAll();
        }

        protected override void SetupGivens()
        {
            GivenAValidActivationContext();
        }
    }
}
