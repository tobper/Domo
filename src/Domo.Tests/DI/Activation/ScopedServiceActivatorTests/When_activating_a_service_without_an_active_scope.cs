using System;
using Domo.DI;
using NUnit.Framework;

namespace Domo.Tests.DI.Activation.ScopedServiceActivatorTests
{
    [TestFixture]
    public class When_activating_a_service_without_an_active_scope : ScopedServiceActivatorTests
    {
        [Test]
        public void An_exception_should_be_thrown()
        {
        }

        protected override Type GetExpectedException()
        {
            return typeof(ServiceScopeHasNotBeenDefinedException);
        }

        protected override void SetupGivens()
        {
            GivenAnActivationContextWithoutAScopedCache();
        }
    }
}